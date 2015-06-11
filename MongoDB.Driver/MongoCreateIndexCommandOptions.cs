/* Copyright 2010-2014 MongoDB Inc.
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

using System;

namespace MongoDB.Driver
{
    /// <summary>
    /// Represents the options to use for an Insert or InsertBatch operation.
    /// </summary>
    public class MongoCreateIndexCommandOptions : IMongoIndexOptions
    {
        // private fields
        private bool _background;
        private bool _unique;
        //private string _name;

        // constructors
        /// <summary>
        /// Initializes a new instance of the MongoInsertOptions class.
        /// </summary>
        public MongoCreateIndexCommandOptions()
        {
            _background = false;
            _unique = false;
        }

        /// <summary>
        /// Seta se o index vai ser criado em background | Por default esta propriedade é false
        /// </summary>
        public bool background
        {
            get { return _background; }
            set { _background = value; }
        }

        /// <summary>
        /// Seta se o index é unico | Por default esta propriedade é false
        /// </summary>
        public bool unique
        {
            get { return _unique; }
            set { _unique = value; }
        }
    }
}
