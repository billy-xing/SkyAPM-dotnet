﻿/*
 * Licensed to the SkyAPM under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The SkyAPM licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using SkyApm.Diagnostics.Logging;

namespace Sky.Apm.Sample.Logging
{
    public class Test
    {
        private readonly ILogger _logger;
        private readonly ISkyApmLogger<Test> _skyApmlogger;

        public Test(ILogger<Test> logger, ISkyApmLogger<Test> skyApmlogger)
        {
            _logger = logger;
            _skyApmlogger = skyApmlogger;
        }

        public void Create()
        {
            _logger.LogError("创建了一个用户对象",new List<string> { "asdasdas","3ewqeqdad","q34asdase2qq"});
            _skyApmlogger.Error($"创建了一个用户对象",new Exception("创建用户对象") );
        }
    }
}
