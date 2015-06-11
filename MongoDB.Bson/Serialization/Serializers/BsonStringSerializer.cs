﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/


namespace MongoDB.Bson.Serialization.Serializers
{
    /// <summary>
    /// Represents a serializer for BsonStrings.
    /// </summary>
    public class BsonStringSerializer : BsonValueSerializerBase<BsonString>
    {
        // private static fields
        private static BsonStringSerializer __instance = new BsonStringSerializer();

        // constructors
        /// <summary>
        /// Initializes a new instance of the BsonStringSerializer class.
        /// </summary>
        public BsonStringSerializer()
            : base(BsonType.String)
        {
        }

        // public static properties
        /// <summary>
        /// Gets an instance of the BsonStringSerializer class.
        /// </summary>
        public static BsonStringSerializer Instance
        {
            get { return __instance; }
        }

        // protected methods
        /// <summary>
        /// Deserializes a value.
        /// </summary>
        /// <param name="context">The deserialization context.</param>
        /// <returns>An object.</returns>
        protected override BsonString DeserializeValue(BsonDeserializationContext context)
        {
            var bsonReader = context.Reader;

            // Gambiarra para que seja possivel desserializar int e double para string
            // Criada por Gmps em 13/08/2014
            switch (bsonReader.CurrentBsonType)
            {
                case BsonType.Int32:
                    return new BsonString(bsonReader.ReadInt32().ToString());
                case BsonType.Int64:
                    return new BsonString(bsonReader.ReadInt64().ToString());
                case BsonType.Double:
                    return new BsonString(bsonReader.ReadDouble().ToString());
                case BsonType.String:
                    return new BsonString(bsonReader.ReadString());
                default :
                    return new BsonString(bsonReader.ReadString());
            }


        }

        /// <summary>
        /// Serializes a value.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        /// <param name="value">The object.</param>
        protected override void SerializeValue(BsonSerializationContext context, BsonString value)
        {
            var bsonWriter = context.Writer;
            bsonWriter.WriteString(value.Value);
        }
    }
}
