﻿namespace Uri.Grammar
{
    using System.Diagnostics.Contracts;

    using Text.Scanning;

    public class RelativePartLexer : Lexer<RelativePart>
    {
        private readonly ILexer<Authority> authorityLexer;

        private readonly ILexer<PathAbsoluteOrEmpty> pathAbsoluteOrEmptyLexer;

        private readonly ILexer<PathAbsolute> pathAbsoluteLexer;

        private readonly ILexer<PathNoScheme> pathNoSchemeLexer;

        private readonly ILexer<PathEmpty> pathEmptyLexer;

        public RelativePartLexer()
            : this(new AuthorityLexer(), new PathAbsoluteOrEmptyLexer(), new PathAbsoluteLexer(), new PathNoSchemeLexer())
        {
        }

        public RelativePartLexer(ILexer<Authority> authorityLexer, ILexer<PathAbsoluteOrEmpty> pathAbsoluteOrEmptyLexer, ILexer<PathAbsolute> pathAbsoluteLexer, ILexer<PathNoScheme> pathNoSchemeLexer)
            : base("relative-part")
        {
            Contract.Requires(authorityLexer != null);
            Contract.Requires(pathAbsoluteOrEmptyLexer != null);
            Contract.Requires(pathAbsoluteLexer != null);
            Contract.Requires(pathNoSchemeLexer != null);
            this.authorityLexer = authorityLexer;
            this.pathAbsoluteOrEmptyLexer = pathAbsoluteOrEmptyLexer;
            this.pathAbsoluteLexer = pathAbsoluteLexer;
            this.pathNoSchemeLexer = pathNoSchemeLexer;
        }

        public override bool TryRead(ITextScanner scanner, out RelativePart element)
        {
            if (scanner.EndOfInput)
            {
                element = default(RelativePart);
                return false;
            }

            var context = scanner.GetContext();
            if (this.TryRead1(scanner, out element))
            {
                return true;
            }

            if (this.TryRead2(scanner, out element))
            {
                return true;
            }

            if (this.TryRead2(scanner, out element))
            {
                return true;
            }

            if (this.TryRead3(scanner, out element))
            {
                return true;
            }

            element = new RelativePart(string.Empty, context);
            return true;
        }

        private bool TryRead1(ITextScanner scanner, out RelativePart element)
        {
            if (scanner.EndOfInput)
            {
                element = default(RelativePart);
                return false;
            }

            var context = scanner.GetContext();
            Element slashes;
            if (!this.TryReadDoubleForwardSlash(scanner, out slashes))
            {
                element = default(RelativePart);
                return false;
            }

            Authority authority;
            if (!this.authorityLexer.TryRead(scanner, out authority))
            {
                scanner.PutBack(slashes.Data);
                element = default(RelativePart);
                return false;
            }

            PathAbsoluteOrEmpty path;
            if (!this.pathAbsoluteOrEmptyLexer.TryRead(scanner, out path))
            {
                scanner.PutBack(authority.Data);
                scanner.PutBack(slashes.Data);
                element = default(RelativePart);
                return false;
            }

            element = new RelativePart(string.Concat(slashes, authority, path), context);
            return true;
        }

        private bool TryRead2(ITextScanner scanner, out RelativePart element)
        {
            if (scanner.EndOfInput)
            {
                element = default(RelativePart);
                return false;
            }

            var context = scanner.GetContext();
            PathAbsolute path;
            if (this.pathAbsoluteLexer.TryRead(scanner, out path))
            {
                element = new RelativePart(path.Data, context);
                return true;
            }

            element = default(RelativePart);
            return false;
        }

        private bool TryRead3(ITextScanner scanner, out RelativePart element)
        {
            if (scanner.EndOfInput)
            {
                element = default(RelativePart);
                return false;
            }

            var context = scanner.GetContext();
            PathNoScheme path;
            if (this.pathNoSchemeLexer.TryRead(scanner, out path))
            {
                element = new RelativePart(path.Data, context);
                return true;
            }

            element = default(RelativePart);
            return false;
        }

        private bool TryReadDoubleForwardSlash(ITextScanner scanner, out Element element)
        {
            if (scanner.EndOfInput)
            {
                element = default(Element);
                return false;
            }

            var context = scanner.GetContext();
            Element slash1;
            if (!this.TryReadForwardSlash(scanner, out slash1))
            {
                element = default(Element);
                return false;
            }

            Element slash2;
            if (!this.TryReadForwardSlash(scanner, out slash2))
            {
                scanner.PutBack(slash1.Data);
                element = default(Element);
                return false;
            }

            element = new Element(string.Concat(slash1, slash2), context);
            return true;
        }

        private bool TryReadForwardSlash(ITextScanner scanner, out Element element)
        {
            if (scanner.EndOfInput)
            {
                element = default(Element);
                return false;
            }

            var context = scanner.GetContext();
            if (scanner.TryMatch('/'))
            {
                element = new Element("/", context);
                return true;
            }

            element = default(Element);
            return false;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.authorityLexer != null);
            Contract.Invariant(this.pathAbsoluteOrEmptyLexer != null);
            Contract.Invariant(this.pathAbsoluteLexer != null);
            Contract.Invariant(this.pathNoSchemeLexer != null);
            Contract.Invariant(this.pathEmptyLexer != null);
        }
    }
}