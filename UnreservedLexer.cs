﻿using System.Diagnostics.Contracts;
using Text.Scanning;
using Text.Scanning.Core;

namespace Uri.Grammar
{
    public class UnreservedLexer : Lexer<UnreservedToken>
    {
        private readonly ILexer<AlphaToken> alphaLexer;
        private readonly ILexer<DigitToken> digitLexer;

        public UnreservedLexer(ILexer<AlphaToken> alphaLexer, ILexer<DigitToken> digitLexer)
        {
            Contract.Requires(alphaLexer != null);
            Contract.Requires(digitLexer != null);
            this.alphaLexer = alphaLexer;
            this.digitLexer = digitLexer;
        }

        public override UnreservedToken Read(ITextScanner scanner)
        {
            var context = scanner.GetContext();
            UnreservedToken token;
            if (this.TryRead(scanner, out token))
            {
                return token;
            }

            throw new SyntaxErrorException(context, "Expected 'unreserved'");
        }

        public override bool TryRead(ITextScanner scanner, out UnreservedToken token)
        {
            if (scanner.EndOfInput)
            {
                token = default(UnreservedToken);
                return false;
            }

            var context = scanner.GetContext();
            AlphaToken alpha;
            if (this.alphaLexer.TryRead(scanner, out alpha))
            {
                token = new UnreservedToken(alpha, context);
                return true;
            }

            DigitToken digit;
            if (this.digitLexer.TryRead(scanner, out digit))
            {
                token = new UnreservedToken(digit, context);
                return true;
            }

            foreach (var c in new[] {'-', '.', '_', '~'})
            {
                if (scanner.TryMatch(c))
                {
                    token = new UnreservedToken(c, context);
                    return true;
                }
            }

            token = default(UnreservedToken);
            return false;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.alphaLexer != null);
            Contract.Invariant(this.digitLexer != null);
        }
    }
}