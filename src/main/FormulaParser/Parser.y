%namespace FormulaParser

%visibility public

%YYSTYPE IToken

%token NUMBER FIDENT RNAME RREF SPACE STRING
%token LB RB SEP

%left EQ GT LT GEQ LEQ NEQ
%left PLUS MINUS CONCAT
%left MULT DIV
%left PERCENT
%left EXP
%left UMINUS
%left UNION
%left INTC
%left RANGE

%%

formula :   expr
            {
              Result = _semantics.InterpretExpression($1);
            }
        ;

/**************************
 * Range Definition       *
 **************************/

range   :   range1
        |   range intcOp range1
            {
              string r1 = ((RangeToken) $1).Value;
              string r2 = ((RangeToken) $3).Value;
              $$ = new RangeToken(r1 + " " + r2);
            }
        ;

range1  :   range2
        |   range1 rangeOp range2
            {
              string r1 = ((RangeToken) $1).Value;
              string r2 = ((RangeToken) $3).Value;
              $$ = new RangeToken(r1 + ":" + r2);
            }
        |   NUMBER rangeOp NUMBER
            {
              // TODO: integer conversion
              string r1 = $1.ToString();
              string r2 = $3.ToString();
              $$ = new RangeToken(r1 + ":" + r2);
            }
        ;

/* There is no support for parenthesis in range operations in Excel. */
range2  :   rRef
      //|   LB range RB
        ;

rRef    :   RREF
        |   RNAME
        |   FIDENT
            {
              $$ = new RangeToken(((IdToken) $1).Value);
            }
        ;

rangeOp :   RANGE
        ;

intcOp  :   /* empty */
        ;

/* There is no union operator, despite what Micrsoft says
   (it is just conceptual)
unionOp :   SEP
        ;
*/

/**************************
 * Expression Definition  *
 **************************/

expr    :   LB expr RB
            {
              $$ = $2;
            }
        |   expr EQ expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.EQ);
            }
        |   expr GT expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.GT);
            }
        |   expr LT expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.LT);
            }
        |   expr GEQ expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.GEQ);
            }
        |   expr LEQ expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.LEQ);
            }
        |   expr NEQ expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.LEQ);
            }
        |   expr PLUS expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.PLUS);
            }
        |   expr MINUS expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.MINUS);
            }
        |   expr MULT expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.MULT);
            }
        |   expr DIV expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.DIV);
            }
        |   expr PERCENT
            {
              $$ = _semantics.InterpretExpression($1, Tokens.PERCENT);
            }
        |   expr EXP expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.EXP);
            }
        |   MINUS expr %prec UMINUS
            {
              $$ = _semantics.InterpretExpression($2, Tokens.UMINUS);
            }
        |   expr CONCAT expr
            {
              $$ = _semantics.InterpretExpression($1, $3, Tokens.CONCAT);
            }
        |   func
        |   range
        |   NUMBER
        |   STRING
        ;

/**************************
 * Function Definition    *
 **************************/

func    :   FIDENT LB RB
            {
              string functionName = ((IdToken) $1).Value;
              IToken[] args = {};
              $$ = _semantics.Function(functionName, args);
            }
        |   FIDENT LB plist RB
            {
              string functionName = ((IdToken) $1).Value;
              IToken[] args = _tempParamListStack.Pop().ToArray();
              $$ = _semantics.Function(functionName, args);
            }
        ;

plist   :   param
            {
              _tempParamListStack.Push(new List<IToken>());
              _tempParamListStack.Peek().Add($1);
            }
        |   plist SEP param
            {
              _tempParamListStack.Peek().Add($3);
            }
        ;

param   :   expr
        ;

%%

  private IToken _result;
  private SemanticsBase _semantics;
  private Stack<List<IToken>> _tempParamListStack = null;

  public IToken
  Result {
    get {
      return _result;
    }
    private set {
      _result = value;
    }
  }

  public
  Parser(SemanticsBase semantics): base(null) {
    _tempParamListStack = new Stack<List<IToken>>();
    _semantics = semantics;
  }

  public
  Parser(string str, SemanticsBase semantics): this(semantics) {
    Scanner scanner = new Scanner();
    scanner.SetSource(str, 0);
    this.Scanner = scanner;
  }
