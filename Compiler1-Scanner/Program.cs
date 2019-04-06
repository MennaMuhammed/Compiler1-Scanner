using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Compiler1_Scanner
{
    class Program
    {

        static StreamWriter outputStream = new StreamWriter("output.txt");
        enum ReservedWords
        {
            WRITE,
            READ,
            IF,
            ELSE,
            RETURN,
            BEGIN,
            END,
            MAIN,
            STRING,
            INT,
            REAL,
            THEN,
            REPEAT,
            UNTIL
        }
        enum tokenType
        {
            reservedWord,
            identifier,
            assign,
            equal,
            notEqual,
            add,
            multiply,
            subtract,
            divide
        }
        static char[] separator = { ' ',';', ',','(',')' };
        static char[] comments = { '{', '}' };
        static char[] operators = { '+', '-', '*','/','<','>'}; 
        static char[] multiOperators = {':','=','!' };
        static char multiOperators1 = '=';
        static string identifier = @"[a-z A-Z]([a-z A-Z]|[0-9])*";
        static string digits = @"[0-9]*";
        static string TokenType(string line)
        {
            
            foreach (string x in Enum.GetNames(typeof(ReservedWords)))
            {
                if(line == x || line == x.ToLower())
                {
                    return "reserved word";
                }
            }

            Match match = Regex.Match(line, identifier);
            if (match.Value.Length == line.Length)
            {
                return "identifier";
            }
            match = Regex.Match(line, digits);
            if (match.Value.Length == line.Length)
            {
                return "number";
            }
            return "not identified";
        }
        static void Token(string expres)
        {
            
            string temp = "";
            bool cont = true;
            for (int i = 0; i < expres.Length; i++)
            {
                if(expres[i] == '{')
                {
                    outputStream.WriteLine(expres[i] + " ," + "Comment start");
                    int x = expres.IndexOf('}');
                    if(x != -1)
                    {

                        i = x;
                        outputStream.WriteLine(expres[i] + " ," + "Comment end");
                    }
                    else
                    {
                        i = expres.Length;
                        cont = false;
                    }
                    
                }
                else if(expres[i] == '}')
                {
                    outputStream.WriteLine(expres[i] + " ," + "Comment end");
                }
                else
                {

                    for (int j = 0; j < separator.Length && cont; j++)
                    {
                        
                        if(expres[i] == separator[j])
                        {
                            if (temp != " " && temp!="")
                            {

                                outputStream.WriteLine(temp + " ," + TokenType(temp));
                            }
                            temp = "";
                            if(expres[i] != ' ')
                            {
                                outputStream.WriteLine(expres[i] + " ," + "special Characters");
                            }
                            cont = false;
                            
                        }
                    }
                    for (int j = 0; j < operators.Length && cont; j++)
                    {
                        if(expres[i] == operators[j])
                        {
                            cont = false;
                            if (temp != " " && temp != "")
                            {

                                outputStream.WriteLine(temp + " ," + TokenType(temp));
                            }
                            temp = "";
                            switch (expres[i])
                            {
                                case '+':
                                    outputStream.WriteLine(expres[i] + " ," + "add");
                                    break;
                                case '-':
                                    outputStream.WriteLine(expres[i] + " ," + "subtract");
                                    break;
                                case '*':
                                    outputStream.WriteLine(expres[i] + " ," + "multiply");
                                    break;
                                case '/':
                                    outputStream.WriteLine(expres[i] + " ," + "divide");
                                    break;
                                case '>':
                                    outputStream.WriteLine(expres[i] + " ," + "greater than");
                                    break;
                                case '<':
                                    outputStream.WriteLine(expres[i] + " ," + "smaller than");
                                    break;
                            }
                        }
                    }
                    for (int j = 0; j < multiOperators.Length && cont; j++)
                    {
                        

                        if (expres[i] == multiOperators[j])
                        {
                            cont = false;
                            //e3ml temp 3bara 3n eh
                            if(temp!=" " && temp != "")
                            {

                                outputStream.WriteLine(temp + " ," + TokenType(temp));
                            }
                            temp = "";
                            switch (expres[i])
                            {
                                case ':':
                                    outputStream.WriteLine(expres[i]+""+expres[i+1] + " ," + "assign");
                                    break;
                                case '=':
                                    outputStream.WriteLine(expres[i] + "" + " ," + "equal");
                                    break;
                                case '!':
                                    outputStream.WriteLine(expres[i] + "" + expres[i + 1] + " ," + "notequal");
                                    break;
                            }
                            i++;
                        }
                    }
                    if(i+1 == expres.Length && cont)
                    {
                        temp = temp + expres[i];
                        if (temp != " " && temp != "")
                        {

                            outputStream.WriteLine(temp + " ," + TokenType(temp));
                        }
                        temp = "";
                    }
                }
                if (cont)
                {
                    temp = temp + expres[i];
                }
                else
                {
                    cont = true;
                }
            }
        }
        static void Main(string[] args)
        {
            
            Console.WriteLine("Enter Input File Location/name (e.g. D:/Input.txt):");
            string fileLocation = Console.ReadLine();
            String line;
            try
            {
                //Pass the file path and file name 
                StreamReader inputStream = new StreamReader(fileLocation);
                //Read the first line of text
                line = inputStream.ReadLine();

                //Continue to read until you reach end of file
               while (line != null)
                {
                    //Token function
                    Token(line);
                    //Read the next line
                    line = inputStream.ReadLine();
                }

                //close the file
                inputStream.Close();
                outputStream.Close();
                Console.WriteLine("Output in file 'output.txt'\nPress enter to exit...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.ReadLine();
            }
        }
    }
}
