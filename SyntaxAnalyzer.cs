
using comp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Comp
{
    class SyntaxAnalyzer
    {
        private LexicalAnalyzer _lex;
        private byte _symbol;
        public SyntaxAnalyzer(LexicalAnalyzer lex)
        {
            _lex = lex;
            NextSym();
        }
        private void NextSym()
        {
            _symbol = _lex.NextSym(); // тек токен
        }
        private void accept(byte symExp, byte err)
        {
            if (_symbol == symExp)
            {
                NextSym();
            }
            else
            {
                InputOutput.AddError(err, InputOutput.PositionNow); // фикс ошибку
            }
        }
        private bool Belong(byte element, HashSet<byte> set)
        {
            return set != null && set.Contains(element);
        }

        private void SkipTo(HashSet<byte> where)
        {
            while (_symbol != 0 && !Belong(_symbol, where))
            {
                NextSym();
            }
        }

        private void SkipTo2(HashSet<byte> start, HashSet<byte> follow)
        {
            while (_symbol != 0 && !Belong(_symbol, start) && !Belong(_symbol, follow))
            {
                NextSym();
            }
        }


        public void Program()
        {
            accept(LexicalAnalyzer.programsy, 214);
            accept(LexicalAnalyzer.ident, 205);
            accept(LexicalAnalyzer.semicolon, 207);

            Block();
        }

        public void Block()
        {
            Type();
            Var();
            Operator();
        }

        private void Type()
        {
            if (_symbol != LexicalAnalyzer.typesy)
            {
                return;
            }
            NextSym();
            TypeDec();
            accept(LexicalAnalyzer.semicolon, 207);
            while (_symbol == LexicalAnalyzer.ident)
            {
                TypeDec();
                accept(LexicalAnalyzer.semicolon, 207);
            }
        }
        private void TypeDec()
        {
            accept(LexicalAnalyzer.ident, 205);
            accept(LexicalAnalyzer.equal, 213);
            TypeSpec();
        }
        private void TypeSpec()
        {
            if (_symbol == LexicalAnalyzer.recordsy)
            {
                RecordType();
            }
            else
            {
                SimpleType();
            }
        }
        private void RecordType()
        {
            accept(LexicalAnalyzer.recordsy, 213);
            while (_symbol == LexicalAnalyzer.ident)
            {
                IdentList();
                accept(LexicalAnalyzer.colon, 206);
                SimpleType();
                accept(LexicalAnalyzer.semicolon, 207);
            }
            accept(LexicalAnalyzer.endsy, 209);
        }
        private void IdentList()
        {
            accept(LexicalAnalyzer.ident, 205);
            while (_symbol == LexicalAnalyzer.comma)
            {
                NextSym();
                accept(LexicalAnalyzer.ident, 205);
            }
        }
        private void SimpleType()
        {
            accept(LexicalAnalyzer.ident, 208);
        }

        private void Var()
        {
            if (_symbol != LexicalAnalyzer.varsy)
            {
                return;
            }
            NextSym();
            VarDec();
            accept(LexicalAnalyzer.semicolon, 207);
            while (_symbol == LexicalAnalyzer.ident)
            {
                VarDec();
                accept(LexicalAnalyzer.semicolon, 207);
            }
        }
        private void VarDec()
        {
            IdentList();
            accept(LexicalAnalyzer.colon, 206);
            TypeSpec();
        }
        private void Operator()
        {
            if (_symbol == LexicalAnalyzer.beginsy)
            {
                CompOperator();
            }
            else if (_symbol == LexicalAnalyzer.withsy)
            {
                WithOperator();
            }
            else if (_symbol == LexicalAnalyzer.ident)
            {
                AssignOperator();
            }
            else
            {
                InputOutput.AddError(213, InputOutput.PositionNow);
            }
        }
        private void CompOperator()
        {
            accept(LexicalAnalyzer.beginsy, 210);
            Operator();
            while (_symbol == LexicalAnalyzer.semicolon)
            {
                NextSym();
                if (_symbol == LexicalAnalyzer.endsy)
                    break;
                Operator();
            }
            accept(LexicalAnalyzer.endsy, 209);
        }
        private void Variable()
        {
            accept(LexicalAnalyzer.ident, 205);
            while (_symbol == LexicalAnalyzer.point)
            {
                NextSym();
                accept(LexicalAnalyzer.ident, 205);
            }
        }
        private void AssignOperator()
        {
            Variable();
            accept(LexicalAnalyzer.assign, 211);
            expression();
        }
        private void WithOperator()
        {
            accept(LexicalAnalyzer.withsy, 212);
            Variable();
            accept(LexicalAnalyzer.dosy, 213);
            Operator();
        }
        private void expression()
        {
            if (_symbol == LexicalAnalyzer.ident)
            {
                Variable();
            }
            else if (_symbol == LexicalAnalyzer.intc)
            {
                NextSym();
            }
            else
            {
                InputOutput.AddError(213, InputOutput.PositionNow);
            }
        }
    }
}