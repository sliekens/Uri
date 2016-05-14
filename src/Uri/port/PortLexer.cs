﻿using System;
using Txt.Core;
using Txt.ABNF;

namespace Uri.port
{
    public sealed class PortLexer : Lexer<Port>
    {
        private readonly ILexer<Repetition> innerLexer;

        public PortLexer(ILexer<Repetition> innerLexer)
        {
            if (innerLexer == null)
            {
                throw new ArgumentNullException(nameof(innerLexer));
            }
            this.innerLexer = innerLexer;
        }

        public override ReadResult<Port> ReadImpl(ITextScanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException(nameof(scanner));
            }
            var result = innerLexer.Read(scanner);
            if (result.Success)
            {
                return ReadResult<Port>.FromResult(new Port(result.Element));
            }
            return ReadResult<Port>.FromSyntaxError(SyntaxError.FromReadResult(result, scanner.GetContext()));
        }
    }
}
