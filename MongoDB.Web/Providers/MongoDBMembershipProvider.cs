using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MongoDB.Web.Providers
{
    public class MongoDBMembershipProvider : MembershipProvider
    {
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private int maxInvalidPasswordAttempts;
        private int minRequiredNonAlphanumericCharacters;
        private int minRequiredPasswordLength;

        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        private string passwordStrengthRegularExpression;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;

        private MongoCollection mongoCollection;

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get { return this.enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return this.enablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return this.maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return this.minRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return this.minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return this.passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return this.passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return this.passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return this.requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return this.requiresUniqueEmail; }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var regex = new Regex(username, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

            if (!this.VerifyPassword(bsonDocument, oldPassword))
            {
                return false;
            }

            var validatePasswordEventArgs = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(validatePasswordEventArgs);

            if (validatePasswordEventArgs.Cancel)
            {
                throw new MembershipPasswordException(validatePasswordEventArgs.FailureInformation.Message);
            }

            var update = Update.Set("LastPasswordChangedDate", DateTime.Now).Set("Password", EncodePassword(newPassword, this.PasswordFormat, bsonDocument["Salt"].AsString));
            this.mongoCollection.Update(query, update);

            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            var regex = new Regex(username, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

            if (!this.VerifyPassword(bsonDocument, password))
            {
                return false;
            }

            var update = Update.Set("PasswordQuestion", newPasswordQuestion).Set("PasswordAnswer", this.EncodePassword(newPasswordAnswer, this.PasswordFormat, bsonDocument["Salt"].AsString));
            return this.mongoCollection.Update(query, update).Ok;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (providerUserKey != null)
            {
                if (!(providerUserKey is Guid))
                {
                    status = MembershipCreateStatus.InvalidProviderUserKey;
                    return null;
                }
            }
            else
            {
                providerUserKey = Guid.NewGuid();
            }

            var validatePasswordEventArgs = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(validatePasswordEventArgs);

            if (validatePasswordEventArgs.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (this.RequiresQuestionAndAnswer && !String.IsNullOrWhiteSpace(passwordQuestion))
            {
                status = MembershipCreateStatus.InvalidQuestion;
                return null;
            }

            if (this.RequiresQuestionAndAnswer && !String.IsNullOrWhiteSpace(passwordAnswer))
            {
                status = MembershipCreateStatus.InvalidAnswer;
                return null;
            }

            if (this.GetUser(username, false) != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            if (this.GetUser(providerUserKey, false) != null)
            {
                status = MembershipCreateStatus.DuplicateProviderUserKey;
                return null;
            }

            if (this.RequiresUniqueEmail && !String.IsNullOrWhiteSpace(this.GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            byte[] buffer = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buffer);
            var salt = Convert.ToBase64String(buffer);

            var creationDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(passwordQuestion))
            {
                passwordQuestion = "wololo";
                passwordAnswer = "wololo";
            }

            var passEncode = this.EncodePassword(password, this.PasswordFormat, salt);
            var passAnswerEncode = this.EncodePassword(passwordAnswer, this.PasswordFormat, salt);

            var dic = new Dictionary<string, object>()
            {
                { "_id", providerUserKey.ToString() },
                { "ApplicationName", this.ApplicationName },
                { "CreationDate", creationDate },
                { "Email", email },
                { "FailedPasswordAnswerAttemptCount", 0 },
                { "FailedPasswordAnswerAttemptWindowStart", creationDate },
                { "FailedPasswordAttemptCount", 0 },
                { "FailedPasswordAttemptWindowStart", creationDate },
                { "IsApproved", isApproved },
                { "IsLockedOut", false },
                { "LastActivityDate", creationDate },
                { "LastLockoutDate", new DateTime(1970, 1, 1) },
                { "LastLoginDate", creationDate },
                { "LastPasswordChangedDate", creationDate },
                { "Password", passEncode },
                { "PasswordAnswer", passAnswerEncode },
                { "PasswordQuestion", passwordQuestion },
                { "Salt", salt },
                { "Username", username },
            };


            var bsonDocument = new BsonDocument();
            bsonDocument.AddRange(dic);

            this.mongoCollection.Insert(bsonDocument);
            status = MembershipCreateStatus.Success;
            return this.GetUser(username, false);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var regex = new Regex(username, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

            return this.mongoCollection.Remove(query).Ok;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();

            var regex = new Regex(emailToMatch, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.Matches("Email", BsonRegularExpression.Create(regex)));

            totalRecords = (int)this.mongoCollection.FindAs<BsonDocument>(query).Count();

            foreach (var bsonDocument in this.mongoCollection.FindAs<BsonDocument>(query).SetSkip(pageIndex * pageSize).SetLimit(pageSize))
            {
                membershipUsers.Add(ToMembershipUser(bsonDocument));
            }

            return membershipUsers;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();

            var regex = new Regex(usernameToMatch, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.Matches("Username", BsonRegularExpression.Create(regex)));

            totalRecords = (int)this.mongoCollection.FindAs<BsonDocument>(query).Count();

            foreach (var bsonDocument in this.mongoCollection.FindAs<BsonDocument>(query).SetSkip(pageIndex * pageSize).SetLimit(pageSize))
            {
                membershipUsers.Add(ToMembershipUser(bsonDocument));
            }

            return membershipUsers;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();

            var query = Query.EQ("ApplicationName", this.ApplicationName);
            totalRecords = (int)this.mongoCollection.FindAs<BsonDocument>(query).Count();

            foreach (var bsonDocument in this.mongoCollection.FindAs<BsonDocument>(query).SetSkip(pageIndex * pageSize).SetLimit(pageSize))
            {
                membershipUsers.Add(ToMembershipUser(bsonDocument));
            }

            return membershipUsers;
        }

        public override int GetNumberOfUsersOnline()
        {
            var timeSpan = TimeSpan.FromMinutes(Membership.UserIsOnlineTimeWindow);
            return (int)this.mongoCollection.Count(Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.GT("LastActivityDate", DateTime.UtcNow.Subtract(timeSpan))));
        }

        public override string GetPassword(string username, string answer)
        {
            if (!this.EnablePasswordRetrieval)
            {
                throw new NotSupportedException("This Membership Provider has not been configured to support password retrieval.");
            }

            var regex = new Regex(username, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

            if (this.RequiresQuestionAndAnswer && !this.VerifyPasswordAnswer(bsonDocument, answer))
            {
                throw new MembershipPasswordException("The password-answer supplied is invalid.");
            }

            return this.DecodePassword(bsonDocument["Password"].AsString, this.PasswordFormat);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var regex = new Regex(username, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

            if (bsonDocument == null)
            {
                return null;
            }

            if (userIsOnline == true)
            {
                var update = Update.Set("LastActivityDate", DateTime.Now);
                this.mongoCollection.Update(query, update);
            }

            return ToMembershipUser(bsonDocument);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var query = Query.EQ("_id", providerUserKey.ToString());
            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

            if (bsonDocument == null)
            {
                return null;
            }

            if (userIsOnline == true)
            {
                var update = Update.Set("LastActivityDate", DateTime.Now);
                this.mongoCollection.Update(query, update);
            }

            return ToMembershipUser(bsonDocument);
        }

        public override string GetUserNameByEmail(string email)
        {
            var regex = new Regex(email, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Email", BsonRegularExpression.Create(regex)));

            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);
            return bsonDocument != null ? bsonDocument["Username"].AsString : null;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            this.ApplicationName = config["applicationName"] ?? HostingEnvironment.ApplicationVirtualPath;
            this.enablePasswordReset = Boolean.Parse(config["enablePasswordReset"] ?? "true");
            this.enablePasswordRetrieval = Boolean.Parse(config["enablePasswordRetrieval"] ?? "false");
            this.maxInvalidPasswordAttempts = Int32.Parse(config["maxInvalidPasswordAttempts"] ?? "5");
            this.minRequiredNonAlphanumericCharacters = Int32.Parse(config["minRequiredNonAlphanumericCharacters"] ?? "1");
            this.minRequiredPasswordLength = Int32.Parse(config["minRequiredPasswordLength"] ?? "7");
            this.passwordAttemptWindow = Int32.Parse(config["passwordAttemptWindow"] ?? "10");
            this.passwordFormat = (MembershipPasswordFormat)Enum.Parse(typeof(MembershipPasswordFormat), config["passwordFormat"] ?? "Hashed");
            this.passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"] ?? String.Empty;
            this.requiresQuestionAndAnswer = Boolean.Parse(config["requiresQuestionAndAnswer"] ?? "false");
            this.requiresUniqueEmail = Boolean.Parse(config["requiresUniqueEmail"] ?? "true");

            if (this.PasswordFormat == MembershipPasswordFormat.Hashed && this.EnablePasswordRetrieval)
            {
                throw new ProviderException("Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set enablePasswordRetrieval to false.");
            }

            if (this.mongoCollection == null)
            {
                MongoClient mongoClient = new MongoClient(new MongoUrl(config["connectionString"] + "/" + (config["database"] ?? "ASPNETDB") ?? "mongodb://localhost"));
                //var settings = new MongoDatabaseSettings(mongoClient.GetServer(), config["database"] ?? "ASPNETDB");
                this.mongoCollection = mongoClient.GetServer().GetDatabase(config["database"] ?? "ASPNETDB").GetCollection(config["collection"] ?? "Users");
            }

            //this.mongoCollection.EnsureIndex("ApplicationName");
            //this.mongoCollection.EnsureIndex("ApplicationName", "Email");
            //this.mongoCollection.EnsureIndex("ApplicationName", "Username");

            base.Initialize(name, config);
        }

        public override string ResetPassword(string username, string answer)
        {
            try
            {
                if (!this.EnablePasswordReset)
                {
                    throw new NotSupportedException("This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file.");
                }

                var regex = new Regex(username, RegexOptions.IgnoreCase);
                var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

                var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

                if (this.RequiresQuestionAndAnswer && !this.VerifyPasswordAnswer(bsonDocument, answer))
                {
                    throw new MembershipPasswordException("The password-answer supplied is invalid.");
                }

                var password = Membership.GeneratePassword(this.MinRequiredPasswordLength, this.MinRequiredNonAlphanumericCharacters);
                var update = Update.Set("LastPasswordChangedDate", DateTime.Now).Set("Password", this.EncodePassword(password, this.PasswordFormat, bsonDocument["Salt"].ToString()));

                this.mongoCollection.Update(query, update);

                return password;
            }
            catch
            {
                throw new Exception("ResetPassword");
            }
        }

        public override bool UnlockUser(string username)
        {
            var regex = new Regex(username, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

            var update = Update.Set("FailedPasswordAttemptCount", 0).Set("FailedPasswordAttemptWindowStart", new DateTime(1970, 1, 1)).Set("FailedPasswordAnswerAttemptCount", 0).Set("FailedPasswordAnswerAttemptWindowStart", new DateTime(1970, 1, 1)).Set("IsLockedOut", false).Set("LastLockoutDate", new DateTime(1970, 1, 1));
            return this.mongoCollection.Update(query, update).Ok;
        }

        public override void UpdateUser(MembershipUser user)
        {
            var query = Query.EQ("_id", user.ProviderUserKey.ToString());
            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

            if (bsonDocument == null)
            {
                throw new ProviderException("The user was not found.");
            }

            var update = Update.Set("ApplicationName", this.ApplicationName)
                .Set("Comment", user.Comment)
                .Set("Email", user.Email)
                .Set("IsApproved", user.IsApproved)
                .Set("LastActivityDate", user.LastActivityDate.ToUniversalTime())
                .Set("LastLoginDate", user.LastLoginDate.ToUniversalTime());

            this.mongoCollection.Update(query, update);
        }

        public override bool ValidateUser(string username, string password)
        {
            var regex = new Regex(username, RegexOptions.IgnoreCase);
            var query = Query.And(Query.EQ("ApplicationName", this.ApplicationName), Query.EQ("Username", BsonRegularExpression.Create(regex)));

            var bsonDocument = this.mongoCollection.FindOneAs<BsonDocument>(query);

            if (bsonDocument == null || !bsonDocument["IsApproved"].AsBoolean || bsonDocument["IsLockedOut"].AsBoolean)
            {
                return false;
            }

            if (this.VerifyPassword(bsonDocument, password))
            {
                this.mongoCollection.Update(query, Update.Set("LastLoginDate", DateTime.Now));
                return true;
            }

            this.mongoCollection.Update(query, Update.Inc("FailedPasswordAttemptCount", 1).Set("FailedPasswordAttemptWindowStart", DateTime.Now));
            return false;
        }

        
        #region Private Methods

        private string DecodePassword(string password, MembershipPasswordFormat membershipPasswordFormat)
        {
            switch (passwordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    return password;

                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Hashed passwords cannot be decoded.");

                default:
                    var passwordBytes = Convert.FromBase64String(password);
                    var decryptedBytes = DecryptPassword(passwordBytes);
                    return decryptedBytes == null ? null : Encoding.Unicode.GetString(decryptedBytes, 16, decryptedBytes.Length - 16);
            }
        }

        private string EncodePassword(string password, MembershipPasswordFormat membershipPasswordFormat, string salt)
        {
            if (password == null)
            {
                return null;
            }

            if (membershipPasswordFormat == MembershipPasswordFormat.Clear)
            {
                return password;
            }

            var passwordBytes = Encoding.Unicode.GetBytes(password);
            var saltBytes = Convert.FromBase64String(salt);
            var allBytes = new byte[saltBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, allBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, allBytes, saltBytes.Length, passwordBytes.Length);

            if (membershipPasswordFormat == MembershipPasswordFormat.Hashed)
            {
                return Convert.ToBase64String(HashAlgorithm.Create("SHA1").ComputeHash(allBytes));
            }

            return Convert.ToBase64String(EncryptPassword(allBytes));
        }

        private MembershipUser ToMembershipUser(BsonDocument bsonDocument)
        {
            if (bsonDocument == null)
            {
                return null;
            }

            var comment = bsonDocument.Contains("Comment") ? bsonDocument["Comment"].ToString() : "";
            var email = bsonDocument.Contains("Email") ? bsonDocument["Email"].ToString() : null;
            var passwordQuestion = bsonDocument.Contains("PasswordQuestion") ? bsonDocument["PasswordQuestion"].ToString() : null;


            //var x = bsonDocument["_id"].ToString();
            return new MembershipUser(this.Name, bsonDocument["Username"].AsString, Guid.Parse(bsonDocument["_id"].ToString()), email, passwordQuestion, comment, bsonDocument["IsApproved"].AsBoolean, bsonDocument["IsLockedOut"].AsBoolean, bsonDocument["CreationDate"].AsDateTime, bsonDocument["LastLoginDate"].AsDateTime, bsonDocument["LastActivityDate"].AsDateTime, bsonDocument["LastPasswordChangedDate"].AsDateTime, bsonDocument["LastLockoutDate"].AsDateTime);
        }

        private bool VerifyPassword(BsonDocument user, string password)
        {
            //var aux = user["Password"].AsString;
            //var x = EncodePassword(password, this.PasswordFormat, user["Salt"].AsString);
            return user["Password"].ToString() == EncodePassword(password, this.PasswordFormat, user["Salt"].AsString);
        }

        private bool VerifyPasswordAnswer(BsonDocument user, string passwordAnswer)
        {
            return user["PasswordAnswer"].AsString == EncodePassword(passwordAnswer, this.PasswordFormat, user["Salt"].AsString);
        }

        #endregion
    }
}
