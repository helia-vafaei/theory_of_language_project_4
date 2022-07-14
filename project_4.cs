using System;
using System.Collections.Generic;
using System.Linq;

namespace proj2
{
    class Program2
    {

        public static string applyOp1(char op,double a)
        {
            double result=0;
            if(op=='t')
                result=Math.Tan(a);

            else if(op=='c')
                result=Math.Cos(a);

            else if(op=='s')
                result=Math.Sin(a);      

            else if(op=='a')
                result=Math.Abs(a);

            else if(op=='e')
                result=Math.Exp(a);  

            else if(op=='l')
            {
                if(a<=0)
                    return "INVALID";    
                result=Math.Log(a,Math.E); 
            }

            else if(op=='q')
            {
                if(a<0)
                    return "INVALID";
                result=Math.Sqrt(a);  
            }         

            return result.ToString();                      
        }
        public static string applyOp2(char op,double b, double a)
        {
            double result=0;
            if(op=='+')
                result= a+b;

            else if(op=='-')
                result=a-b ;  

            else if(op=='*')
                result= a*b; 

            else if(op=='/')
            {
                if(b==0)
                    return "INVALID";
                result= a/b; 
            }
            else if(op=='^')
                result= Math.Pow(a,b);

            return result.ToString();           
        }

        public static bool CheckLead(char op1,char op2)
        {
            if (op2 == '(' || op2 == ')')
            {
                return false;
            }
            else if ((op1 == '^' || op1 == '*' || op1 == '/' || op1 == 'c' || op1 == 's' || op1 == 't' || op1 == 'l' || op1 == 'a' || op1 == 'e' || op1 == 'q') && (op2 == '+' || op2 == '-'))
            {
                return false;
            }
            else if ((op1 == '^') && (op2 == '*' || op2 == '/'))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string getting(Stack<char> ops,Stack<double> datas)
        {
            string result="";
            if(ops.Peek()=='+' || ops.Peek()=='*' || ops.Peek()=='-' || ops.Peek()=='/' || ops.Peek()=='^')
            {
                result=applyOp2(ops.Pop(),datas.Pop(),datas.Pop());
            }
            else if(ops.Peek()=='s' || ops.Peek()=='c' || ops.Peek()=='t' || ops.Peek()=='a' || ops.Peek()=='e' || ops.Peek()=='l' || ops.Peek()=='q')
            {
                result=applyOp1(ops.Pop(),datas.Pop());
            }  
            return result;  
        }
        public static string solve(string input)
        {
            double piffling=0;
            char[] inputs = input.ToCharArray();
            Stack<char> ops = new Stack<char>();
            Stack<double> datas = new Stack<double>();
            for (int i = 0; i < inputs.Length; i++)
            {
                if(inputs[i]==' ')
                    continue;

                else if(inputs[i]=='-' && double.TryParse(inputs[i+1].ToString(),out piffling))
                {
                    string tmp="";
                    while (i<inputs.Length && ((double.TryParse(inputs[i].ToString(),out piffling)) || inputs[i]=='.' || inputs[i]=='-'))
                    {
                        tmp+=inputs[i];
                        i++;
                    }
                    datas.Push(double.Parse(tmp));
                }
                else if(double.TryParse(inputs[i].ToString(),out piffling))
                {
                    string tmp="";
                    while (i<inputs.Length && ((double.TryParse(inputs[i].ToString(),out piffling)) || inputs[i]=='.'))
                    {
                        if(inputs[i]=='.' && inputs[i-1] == '.')   // 2..3 - 6
                            return "INVALID";
                        else if(i<inputs.Length-2 && inputs[i+1]==' ' && double.TryParse(inputs[i+2].ToString(),out piffling))  // 2 6 - 2
                            return "INVALID";  
                        tmp+=inputs[i];
                        i++;
                    }
                    datas.Push(double.Parse(tmp));
                    i--;
                }

                else if (inputs[i] == '(')
                {
                    ops.Push(inputs[i]);
                }

                else if (inputs[i] == ')')
                {
                    if(inputs[i-1]=='(')
                        return "INVALID";
                    while (ops.Peek() != '(')
                    {  
                        string result=getting(ops,datas); 
                        if(result=="INVALID")
                            return result;
                        datas.Push(double.Parse(result));
                    }
                    ops.Pop();
                }

                else if(inputs[i]=='*' || inputs[i]=='/' || inputs[i]=='+' || inputs[i]=='-' || inputs[i]=='^')
                {
                    if(i==0)
                        return "INVALID";
                    else if((inputs[i-1]=='*' || inputs[i-1]=='/' || inputs[i-1]=='+' || inputs[i-1]=='-' || inputs[i-1]=='^') || inputs[i-1]!=' ')
                        return "INVALID";
                    else 
                    {
                        while (ops.Count > 0 && CheckLead(inputs[i], ops.Peek()))
                        {
                            string result=applyOp2(ops.Pop(),datas.Pop(),datas.Pop());
                            if(result=="INVALID")
                                return result;
                            datas.Push(double.Parse(result));
                        }
                        ops.Push(inputs[i]);
                    }
                }

                else if((inputs[i]=='s' && inputs[i+1]=='i') || inputs[i]=='c' || inputs[i]=='t' || inputs[i]=='a' || inputs[i]=='e')
                {
                    while (ops.Count > 0 && CheckLead(inputs[i], ops.Peek()))
                    {
                        string result=applyOp1(ops.Pop(),datas.Pop());
                        datas.Push(double.Parse(result));
                    }
                    ops.Push(inputs[i]);
                    i+=2;
                }
                else if(inputs[i]=='l')
                {
                    while (ops.Count > 0 && CheckLead(inputs[i], ops.Peek()))
                    {
                        string result=applyOp1(ops.Pop(),datas.Pop());
                        datas.Push(double.Parse(result));
                    }
                    ops.Push(inputs[i]);
                    i+=1;
                }
                else if(inputs[i]=='s' && inputs[i+1]=='q')
                {
                    while (ops.Count > 0 && CheckLead(inputs[i], ops.Peek()))
                    {
                        string result=applyOp1(ops.Pop(),datas.Pop());
                        datas.Push(double.Parse(result));
                    }
                    ops.Push('q');
                    i+=3;
                }
            }  
            while (ops.Count > 0)
            {
                string result=getting(ops,datas); 
                if(result=="INVALID")
                    return result;
                datas.Push(double.Parse(result));
            }
            return datas.Pop().ToString();  
        }

        public static bool CheckBacket(string input)
        {
            long m=input.Length;
            bool isblanced=true;
            List<char> list=new List<char>();
            foreach(char i in input)   
            {
                if(i=='(')
                    list.Add(i);
                else if(i==')')
                {
                    if(list.Count==0)
                    {
                        isblanced=false;
                        break;
                    }
                    char top=list[list.Count-1];
                    list.RemoveAt(list.Count-1);
                    if(top=='(' && i!=')')
                    {
                        isblanced=false;
                        break;
                    }
                }    
            }
            if(isblanced==true && list.Count==0)
                return true;
            else
                return false;
        }
        static void Main(string[] args)
        {
            string input=Console.ReadLine();
            if(CheckBacket(input))
            {
                double piffling=0;
                string result=solve(input);
                if(double.TryParse(result,out piffling))
                {
                    // double result=double.Parse(solve(input));
                    // if(result.ToString().Contains("."))
                    //     System.Console.WriteLine(Math.Floor(result*100)/100);
                    // else    
                    System.Console.WriteLine(String.Format("{0:0.00}", piffling));
                }    
                else
                    System.Console.WriteLine(result);    
            }
            else 
                System.Console.WriteLine("INVALID");
            
        }
    }
}
