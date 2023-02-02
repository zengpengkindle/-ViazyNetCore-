﻿using System.Collections.Generic;
using System.Linq.Expressions;

namespace ViazyNetCore.Formatter.Excel.Models
{
    public class ReadOptions
    {

        public ReadOptions()
        {
            Converters = new Dictionary<string, LambdaExpression>();
        }

        /// <summary>
        /// Genera opciones por defecto si no se indican al consumir la librería
        /// </summary>
        public static ReadOptions DefaultOptions => new ReadOptions()
        {
            TitlesInFirstRow = true,
            RowStart = 0

        };

        public bool TitlesInFirstRow { get; set; }
        //number row of content table
        public int RowStart { get; set; }

        public Dictionary<string, LambdaExpression> Converters { get; set; }
    }
}
