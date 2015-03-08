﻿using System.Diagnostics.Contracts;


namespace Uri.Grammar
{
    using SLANG;

    public class Reserved : Element
    {
        public Reserved(GenericDelimiter delimiter, ITextContext context)
            : base(delimiter.Data, context)
        {
            Contract.Requires(delimiter != null);
            Contract.Requires(context != null);
        }

        public Reserved(SubcomponentsDelimiter delimiter, ITextContext context)
            : base(delimiter.Data, context)
        {
            Contract.Requires(delimiter != null);
            Contract.Requires(context != null);
        }
    }
}