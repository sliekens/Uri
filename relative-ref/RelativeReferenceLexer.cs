﻿namespace Uri.Grammar
{
    using System;

    using SLANG;

    public class RelativeReferenceLexer : Lexer<RelativeReference>
    {
        private readonly ILexer<Sequence> innerLexer;

        public RelativeReferenceLexer(ILexer<Sequence> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException("innerLexer");
            }

            this.innerLexer = innerLexer;
        }

        public override bool TryRead(ITextScanner scanner, out RelativeReference element)
        {
            Sequence result;
            if (this.innerLexer.TryRead(scanner, out result))
            {
                element = new RelativeReference(result);
                return true;
            }

            element = default(RelativeReference);
            return false;
        }
    }
}