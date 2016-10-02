﻿using System;
using JetBrains.Annotations;
using Txt.ABNF;
using Txt.Core;
using UriSyntax.pchar;

namespace UriSyntax.path_empty
{
    public class PathEmptyLexerFactory : LexerFactory<PathEmpty>
    {
        static PathEmptyLexerFactory()
        {
            Default = new PathEmptyLexerFactory(
                Txt.ABNF.RepetitionLexerFactory.Default,
                pchar.PathCharacterLexerFactory.Default);
        }

        public PathEmptyLexerFactory(
            [NotNull] IRepetitionLexerFactory repetitionLexerFactory,
            [NotNull] ILexerFactory<PathCharacter> pathCharacterLexerFactory)
        {
            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }
            if (pathCharacterLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(pathCharacterLexerFactory));
            }
            RepetitionLexerFactory = repetitionLexerFactory;
            PathCharacterLexerFactory = pathCharacterLexerFactory.Singleton();
        }

        public static PathEmptyLexerFactory Default { get; }

        public ILexerFactory<PathCharacter> PathCharacterLexerFactory { get; }

        public IRepetitionLexerFactory RepetitionLexerFactory { get; }

        public override ILexer<PathEmpty> Create()
        {
            var innerLexer = RepetitionLexerFactory.Create(PathCharacterLexerFactory.Create(), 0, 0);
            return new PathEmptyLexer(innerLexer);
        }
    }
}
