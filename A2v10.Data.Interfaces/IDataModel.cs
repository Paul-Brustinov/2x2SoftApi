﻿// Copyright © 2015-2018 Alex Kukhtin. All rights reserved.

using System;
using System.Collections.Generic;
using System.Dynamic;

namespace A2v10.Data.Interfaces
{
    public interface IDataModel
    {
        ExpandoObject Root { get; }
        ExpandoObject System { get; }
        IDictionary<String, IDataMetadata> Metadata { get; }

        Boolean IsReadOnly { get; }

        T Eval<T>(String expression);
        void Merge(IDataModel src);

        String CreateScript(IDataScripter scripter);
        IDictionary<String, dynamic> GetDynamic();
    }
}
