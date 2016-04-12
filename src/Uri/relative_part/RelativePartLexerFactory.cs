﻿using System;
using Txt;
using Txt.ABNF;
using Uri.authority;
using Uri.path_abempty;
using Uri.path_absolute;
using Uri.path_empty;
using Uri.path_noscheme;

namespace Uri.relative_part
{
    public class RelativePartLexerFactory : ILexerFactory<RelativePart>
    {
        private readonly IAlternativeLexerFactory alternativeLexerFactory;

        private readonly ILexerFactory<Authority> authorityLexerFactory;

        private readonly ILexerFactory<PathAbsolute> pathAbsoluteLexerFactory;

        private readonly ILexerFactory<PathAbsoluteOrEmpty> pathAbsoluteOrEmptyLexerFactory;

        private readonly ILexerFactory<PathEmpty> pathEmptyLexerFactory;

        private readonly ILexerFactory<PathNoScheme> pathNoSchemeLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public RelativePartLexerFactory(
            IAlternativeLexerFactory alternativeLexerFactory,
            IConcatenationLexerFactory concatenationLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<Authority> authorityLexerFactory,
            ILexerFactory<PathAbsoluteOrEmpty> pathAbsoluteOrEmptyLexerFactory,
            ILexerFactory<PathAbsolute> pathAbsoluteLexerFactory,
            ILexerFactory<PathNoScheme> pathNoSchemeLexerFactory,
            ILexerFactory<PathEmpty> pathEmptyLexerFactory)
        {
            if (alternativeLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternativeLexerFactory));
            }

            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }

            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }

            if (authorityLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(authorityLexerFactory));
            }

            if (pathAbsoluteOrEmptyLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(pathAbsoluteOrEmptyLexerFactory));
            }

            if (pathAbsoluteLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(pathAbsoluteLexerFactory));
            }

            if (pathNoSchemeLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(pathNoSchemeLexerFactory));
            }

            if (pathEmptyLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(pathEmptyLexerFactory));
            }

            this.alternativeLexerFactory = alternativeLexerFactory;
            this.concatenationLexerFactory = concatenationLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.authorityLexerFactory = authorityLexerFactory;
            this.pathAbsoluteOrEmptyLexerFactory = pathAbsoluteOrEmptyLexerFactory;
            this.pathAbsoluteLexerFactory = pathAbsoluteLexerFactory;
            this.pathNoSchemeLexerFactory = pathNoSchemeLexerFactory;
            this.pathEmptyLexerFactory = pathEmptyLexerFactory;
        }

        public ILexer<RelativePart> Create()
        {
            var delim = terminalLexerFactory.Create(@"//", StringComparer.Ordinal);
            var authority = authorityLexerFactory.Create();
            var pathAbEmpty = pathAbsoluteOrEmptyLexerFactory.Create();
            var seq = concatenationLexerFactory.Create(delim, authority, pathAbEmpty);
            var pathAbsolute = pathAbsoluteLexerFactory.Create();
            var pathNoScheme = pathNoSchemeLexerFactory.Create();
            var pathEmpty = pathEmptyLexerFactory.Create();
            var innerLexer = alternativeLexerFactory.Create(seq, pathAbsolute, pathNoScheme, pathEmpty);
            return new RelativePartLexer(innerLexer);
        }
    }
}