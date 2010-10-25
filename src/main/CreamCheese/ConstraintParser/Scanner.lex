%using CreamCheese.ConstraintParser;
%namespace CreamCheese.ConstraintParser

%visibility internal

%option nofiles

digit [0-9]
space [ \t\n\v\f\r]
notspace [^ \t\n\v\f\r]

%%

/* Separators */
"("  { return (int) Tokens.LB; }
")"  { return (int) Tokens.RB; }
","  { return (int) Tokens.SEP; }

/* Comparison Operators */
"="  { return (int) Tokens.EQ; }
">"  { return (int) Tokens.GT; }
"<"  { return (int) Tokens.LT; }
">=" { return (int) Tokens.GEQ; }
"<=" { return (int) Tokens.LEQ; }
"<>" { return (int) Tokens.NEQ; }

/* Arithmetic Operators */
"+"  { return (int) Tokens.PLUS; }
"-"  { return (int) Tokens.MINUS; }
"*"  { return (int) Tokens.MULT; }
"/"  { return (int) Tokens.DIV; }
"%"  { return (int) Tokens.PERCENT; }
"^"  { return (int) Tokens.EXP; }

/* Range Operators */
":"  { return (int) Tokens.RANGE; }
// Omitting the intersection operator: (" ", "!").
// Omitting the union operator: (",").

/* String Literal */
\"(\"\"|[^\"])*\" {
  yylval = new ExpressionTree(new StringToken(yytext));
  return (int) Tokens.STRING;
}

/* Numeric Literal */
{digit}*"."?{digit}+ {
  int number = int.Parse(yytext);
  yylval = new ExpressionTree(new NumberToken(number));
  return (int) Tokens.NUMBER;
}

/* Function Indentifier */
"_"+[a-zA-Z0-9][a-zA-Z0-9_]*|[a-zA-Z][a-zA-Z0-9_]* {
  yylval = new ExpressionTree(new UnknownToken(yytext));
  return (int) Tokens.FIDENT;
}

/* Range Reference */
(((\'(\'\'|[^\'])*\')|[^ \t\n\v\f\r\'!]+)!)?("$"?[a-zA-Z]+)("$"?[0-9]+)? {
  yylval = new ExpressionTree(new RangeToken(yytext));
  return (int) Tokens.RREF;
}

/* Range Name */
[\\_a-zA-Z][a-zA-Z0-9.\\_]* {
  yylval = new ExpressionTree(new RangeToken(yytext));
  return (int) Tokens.RNAME;
}

/* Whitespace */
{space}+ {
  /* Ignore */
}

%%
