﻿namespace Uri.Grammar
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using SLANG;

    public class SegmentNonZeroNoColonsLexer : Lexer<SegmentNonZeroNoColons>
    {
        private readonly ILexer<PercentEncoding> percentEncodingLexer;
        private readonly ILexer<SubcomponentsDelimiter> subcomponentsLexer;
        private readonly ILexer<Unreserved> unreservedLexer;

        public SegmentNonZeroNoColonsLexer()
            : this(new UnreservedLexer(), new PercentEncodingLexer(), new SubcomponentsDelimiterLexer())
        {
        }

        public SegmentNonZeroNoColonsLexer(ILexer<Unreserved> unreservedLexer, 
            ILexer<PercentEncoding> percentEncodingLexer, ILexer<SubcomponentsDelimiter> subcomponentsLexer)
            : base("segment-nz-nc")
        {
            Contract.Requires(unreservedLexer != null);
            Contract.Requires(percentEncodingLexer != null);
            Contract.Requires(subcomponentsLexer != null);
            this.unreservedLexer = unreservedLexer;
            this.percentEncodingLexer = percentEncodingLexer;
            this.subcomponentsLexer = subcomponentsLexer;
        }

        public override bool TryRead(ITextScanner scanner, out SegmentNonZeroNoColons element)
        {
            var elements = new List<Alternative<Unreserved, PercentEncoding, SubcomponentsDelimiter, Element>>();
            var context = scanner.GetContext();
            while (!scanner.EndOfInput)
            {
                var innerContext = scanner.GetContext();
                Alternative<Unreserved, PercentEncoding, SubcomponentsDelimiter, Element> alternative;
                Unreserved unreserved;
                if (this.unreservedLexer.TryRead(scanner, out unreserved))
                {
                    alternative =
                        new Alternative<Unreserved, PercentEncoding, SubcomponentsDelimiter, Element>(unreserved, 1,
                            innerContext);
                }
                else
                {
                    PercentEncoding percentEncoding;
                    if (this.percentEncodingLexer.TryRead(scanner, out percentEncoding))
                    {
                        alternative =
                            new Alternative<Unreserved, PercentEncoding, SubcomponentsDelimiter, Element>(
                                percentEncoding, 2, innerContext);
                    }
                    else
                    {
                        SubcomponentsDelimiter subcomponentsDelimiter;
                        if (this.subcomponentsLexer.TryRead(scanner, out subcomponentsDelimiter))
                        {
                            alternative =
                                new Alternative<Unreserved, PercentEncoding, SubcomponentsDelimiter, Element>(
                                    percentEncoding, 3, innerContext);
                        }
                        else
                        {
                            if (scanner.TryMatch('@'))
                            {
                                var terminal = new Element('@', innerContext);
                                alternative =
                                    new Alternative<Unreserved, PercentEncoding, SubcomponentsDelimiter, Element>(
                                        terminal, 4, innerContext);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                elements.Add(alternative);
            }

            if (elements.Count == 0)
            {
                element = default(SegmentNonZeroNoColons);
                return false;
            }

            element = new SegmentNonZeroNoColons(elements, context);
            return true;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.unreservedLexer != null);
            Contract.Invariant(this.percentEncodingLexer != null);
            Contract.Invariant(this.subcomponentsLexer != null);
        }
    }
}