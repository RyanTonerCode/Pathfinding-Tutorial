﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    public interface IGraphSearcher<T>
    {
        public void Add(T item);
        public T Remove();
        public bool IsEmpty();

    }
}
