%namespace CreamCheese.ConstraintParser

%visibility internal

%YYSTYPE ExpressionTree

%token NUMBER FIDENT RNAME RREF SPACE STRING
%token LB RB SEP

%left EQ GT LT GEQ LEQ NEQ
%left PLUS MINUS
%left MULT DIV
%left PERCENT
%left EXP
%left UMINUS
%left UNION
%left INTC
%left RANGE

%%

list    :   term
            {
				_tree = $1;
				_isParsed = true;
			}
        ;

term    :   expr
        |   STRING
        ;

/**************************
 * Range Definition       *
 **************************/

range   :   range1
        |   range intcOp range1
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Intersect);
				$$ = new ExpressionTree(ot, $1, $3);
			}
        ;

range1  :   range2
        |   range1 rangeOp range2
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Range);
				$$ = new ExpressionTree(ot, $1, $3);
	        }
		|   NUMBER rangeOp NUMBER
		    {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Range);
				RangeToken rt1 = new RangeToken(((NumberToken) $1.Root.Token).Value.ToString());
				$1.Root.Token = rt1;
				RangeToken rt2 = new RangeToken(((NumberToken) $3.Root.Token).Value.ToString());
				$3.Root.Token = rt2;
				$$ = new ExpressionTree(ot, $1, $3);
		    }
        ;

range2  :   rRef
      //|   LB range RB
        ;

rRef    :   RREF
        |   RNAME
        |   FIDENT
            {
		        if($1.Root.Token.Type == TokenType.Unknown) {
					RangeToken rt = new RangeToken(((UnknownToken) $1.Root.Token).Value);
					$$ = new ExpressionTree(rt);
				}
	        }
        ;

rangeOp :   RANGE
        ;

intcOp  :   /* empty */
        ;

/* There is no union operator, despite what Micrsoft says
unionOp :   SEP
        ;
*/

/**************************
 * Expression Definition  *
 **************************/

expr    :   expr EQ expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.EqualTo);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr GT expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.GreaterThan);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr LT expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.LessThan);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr GEQ expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.GreaterThanOrEqualTo);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr LEQ expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.LessThanOrEqualTo);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr NEQ expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.NotEqualTo);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr PLUS expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Plus);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr MINUS expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Minus);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr MULT expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Multiply);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr DIV expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Divide);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   expr PERCENT
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Percent);
				$$ = new ExpressionTree(ot, $1, null);
            }
        |   expr EXP expr
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Exponentiate);
				$$ = new ExpressionTree(ot, $1, $3);
            }
        |   MINUS expr %prec UMINUS
            {
				OperatorToken ot = new OperatorToken(OperatorToken.Operator.Negate);
				$$ = new ExpressionTree(ot, $2, null);
            }
        |   func
        |   range
        |   NUMBER
        ;

/**************************
 * Function Definition    *
 **************************/

func    :   FIDENT LB RB
            {
				if($1.Root.Token.Type == TokenType.Unknown) {
					FunctionToken ft = new FunctionToken(((UnknownToken) $1.Root.Token).Value);
					$$ = new ExpressionTree(ft);
				}
	        }
        |   FIDENT LB plist RB
	        {
				if($1.Root.Token.Type == TokenType.Unknown) {
					FunctionToken ft = new FunctionToken(((UnknownToken) $1.Root.Token).Value, _tempParamList);
					$$ = new ExpressionTree(ft);
				}
				_tempParamList = null;
	        }
        ;

plist   :   param
            {
				_tempParamList = new List<ExpressionTree>();
				_tempParamList.Add($1);
			}
		|   plist SEP param
			{
				_tempParamList.Add($3);
			}
        ;

param   :   term
        ;

%%

  private List<ExpressionTree> _tempParamList = null;
  private bool _isParsed = false;
  private ExpressionTree _tree = null;

  public bool IsParsed {
    get {
      return _isParsed;
    }
  }

  public ExpressionTree Tree {
    get {
      return _tree;
    }
  }

  public Parser(): base(null) {}

  public Parser(string str)
  :base(null) {
    Scanner scanner = new Scanner();
    scanner.SetSource(str, 0);
    this.Scanner = scanner;
  }
