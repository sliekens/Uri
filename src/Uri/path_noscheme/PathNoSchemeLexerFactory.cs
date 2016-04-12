﻿using System;
using Txt;
using Txt.ABNF;
using Uri.segment;
using Uri.segment_nz_nc;

namespace Uri.path_noscheme
{
    public class PathNoSchemeLexerFactory : ILexerFactory<PathNoScheme>
    {
        private readonly IRepetitionLexerFactory repetitionLexerFactory;

        private readonly ILexerFactory<Segment> segmentLexerFactory;

        private readonly ILexerFactory<SegmentNonZeroLengthNoColons> segmentNonZeroLengthNoColonsLexerFactory;

        private readonly IConcatenationLexerFactory concatenationLexerFactory;

        private readonly ITerminalLexerFactory terminalLexerFactory;

        public PathNoSchemeLexerFactory(
            IConcatenationLexerFactory concatenationLexerFactory,
            IRepetitionLexerFactory repetitionLexerFactory,
            ITerminalLexerFactory terminalLexerFactory,
            ILexerFactory<Segment> segmentLexerFactory,
            ILexerFactory<SegmentNonZeroLengthNoColons> segmentNonZeroLengthNoColonsLexerFactory)
        {
            if (concatenationLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(concatenationLexerFactory));
            }

            if (repetitionLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(repetitionLexerFactory));
            }

            if (terminalLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(terminalLexerFactory));
            }

            if (segmentLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(segmentLexerFactory));
            }

            if (segmentNonZeroLengthNoColonsLexerFactory == null)
            {
                throw new ArgumentNullException(nameof(segmentNonZeroLengthNoColonsLexerFactory));
            }

            this.concatenationLexerFactory = concatenationLexerFactory;
            this.repetitionLexerFactory = repetitionLexerFactory;
            this.terminalLexerFactory = terminalLexerFactory;
            this.segmentLexerFactory = segmentLexerFactory;
            this.segmentNonZeroLengthNoColonsLexerFactory = segmentNonZeroLengthNoColonsLexerFactory;
        }

        public ILexer<PathNoScheme> Create()
        {
            // "/"
            var a = terminalLexerFactory.Create(@"/", StringComparer.Ordinal);

            // segment
            var b = segmentLexerFactory.Create();

            // "/" segment
            var c = concatenationLexerFactory.Create(a, b);

            // *( "/" segment )
            var d = repetitionLexerFactory.Create(c, 0, int.MaxValue);

            // segment-nz-nc
            var e = segmentNonZeroLengthNoColonsLexerFactory.Create();

            // segment-nz-nc *( "/" segment )
            var f = concatenationLexerFactory.Create(e, d);

            // path-noscheme
            return new PathNoSchemeLexer(f);
        }
    }
}