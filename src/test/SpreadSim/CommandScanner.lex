%using SpreadSim;
%namespace SpreadSim.CommandParser

%visibility internal

%%

/* Functions */
"set" { return (int) Tokens.SET; }
"show" { return (int) Tokens.SHOW; }

/* Separators */
"("  { return (int) Tokens.LB; }
")"  { return (int) Tokens.RB; }
","  { return (int) Tokens.SEP; }
";"  { return (int) Tokens.CSEP; }

/* String Literal */
\"(\"\"|[^\"])*\" {
  string s = yytext;
  int i = s.IndexOf('"');
  while(i != -1) {
    s = s.Remove(i, 1);
    if(i + 1 < s.Length) {
      i = s.IndexOf('"', i + 1);
    } else {
      break;
    }
  }
  yylval = s;
  return (int) Tokens.STRING;
}

/* Range Reference */
[A-Z][0-9]+ {
  yylval = yytext;
  return (int) Tokens.RREF;
}

%%
