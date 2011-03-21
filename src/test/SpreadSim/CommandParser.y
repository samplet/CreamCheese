%using SpreadSim;
%using FormulaParser;
%namespace SpreadSim.CommandParser

%visibility internal

%YYSTYPE string

%token RREF STRING
%token LB RB SEP CSEP
%token SET SHOW

%%

program :   command CSEP
        |   program command CSEP
        ;

command :   SET LB RREF SEP STRING RB
            {
              _worksheet.Set($3, $5);
              Cell c = _worksheet.GetCell($3);
              if(c.SelfDependent) {
                _worksheet.Set(c.Address, "");
                Console.WriteLine("Error: Did not set cell because its formula contains a circular-refernce.");
              }
            }
        |   SHOW LB RREF RB
            {
              IToken v = _worksheet[$3];
              if(v != null) {
                Console.WriteLine(v.ToString());
              } else {
                Console.WriteLine();
              }
            }
        ;

%%

public Worksheet _worksheet;

public Parser(System.IO.Stream stream)
  : base(null) {
  _worksheet = new Worksheet();
  Scanner = new Scanner(stream);
}
