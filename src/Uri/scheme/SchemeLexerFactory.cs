﻿using System;
using JetBrains.Annotations;
using Txt.ABNF;
using Txt.ABNF.Core.ALPHA;
using Txt.ABNF.Core.DIGIT;
using Txt.Core;

namespace UriSyntax.scheme
{
    public class SchemeLexerFactory : LexerFactory<Scheme>
    {
        static SchemeLexerFactory()
        {
            Default = new SchemeLexerFactory(
                Txt.ABNF.TerminalLexerFactory.Default,
                Txt.ABNF.AlternationLexerFactory.Default,
                Txt.ABNF.ConcatenationLexerFactory.Default,
                Txt.ABNF.RepetitionLexerFactory.Default,
                Txt.ABNF.Core.ALPHA.AlphaLexerFactory.Default,
                Txt.ABNF.Core.DIGIT.DigitLexerFactory.Default);
        }

        public SchemeLexerFactory(
            [NotNull] ITerminalLexerFactory terminalLexerFactory,
            [NotNull] IAlternationLexerFactory alternationLexerFactory,
            [NotNull] IConcatenationLexerFactory concatenationLexerFactory,
            [NotNull] IRepetitionLexerFactory repetitionLexerFactory,
            [NotNull] ILexerFactory<Alpha> alphaLexerFactory,
            [NotNull] ILexerFactory<Digit> digitLexerFactory)
        {
            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }
            if (alternationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alternationLexerFactory));
            }
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (alphaLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(alphaLexerFactory));
            }
            if (digitLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(digitLexerFactory));
            }
            TerminalLexerFactory = terminalLexerFactory;
            AlternationLexerFactory = alternationLexerFactory;
            ConcatenationLexerFactory = concatenationLexerFactory;
            RepetitionLexerFactory = repetitionLexerFactory;
            AlphaLexerFactory = alphaLexerFactory.Singleton();
            DigitLexerFactory = digitLexerFactory.Singleton();
        }

        public static SchemeLexerFactory Default { get; }

        public ILexerFactory<Alpha> AlphaLexerFactory { get; }

        public IAlternationLexerFactory AlternationLexerFactory { get; }

        public IConcatenationLexerFactory ConcatenationLexerFactory { get; }

        public ILexerFactory<Digit> DigitLexerFactory { get; }

        public IRepetitionLexerFactory RepetitionLexerFactory { get; }

        public ITerminalLexerFactory TerminalLexerFactory { get; }

        public override ILexer<Scheme> Create()
        {
            var alpha = AlphaLexerFactory.Create();
            var digit = DigitLexerFactory.Create();
            var innerLexer = ConcatenationLexerFactory.Create(
                alpha,
                RepetitionLexerFactory.Create(
                    AlternationLexerFactory.Create(
                        alpha,
                        digit,
                        TerminalLexerFactory.Create(@"+", StringComparer.Ordinal),
                        TerminalLexerFactory.Create(@"-", StringComparer.Ordinal),
                        TerminalLexerFactory.Create(@".", StringComparer.Ordinal)),
                    0,
                    int.MaxValue));
            return new SchemeLexer(innerLexer);
        }
    }
}
