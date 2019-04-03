compilers-2019
main repository for lab works


##### 1. Short description about my chosen theme  
Program with OCR-system, which loads photo and document, and compares it.   

##### 2. Description of my language (types, built-in functions etc.)   
Types: photo(as string path to photo), document(as string path to document), var(another object).  
Built-in functions: print(parameter), read(parameter), load(parameter).  
Statement: if-else, switch-case.  
Operators: =, ==, !=, >, >=, <, <=.  

##### 3. Description   
Interpreter consists of lexical analyzer, syntax analyzer, semantic analyzer and translator(class `Interpreter`).  
For lexer it uses class `Token`, `TokenKind` and `Lexer`(in folder Lexer).  
For syntax it uses class `Parser`(folder Parser) and different kind of parser,
description of `Expression`, `Statement`, `Declaration` (folder Syntax).  
For semantic it uses `parserTree` and class `Semantic`.  
After all tests use translator.  

##### Example
**Source code:**
```
func main()
{
    photo a;
    photo b;
    photo c;
    a = "E:\test1.png";
    b = "E:\test2.png";
    c = "E:\test1.png";
    switch(c)
    {
         case a: print(a);
         case b: print(b);
    }
}
main();
```
In this example the first thing is creating function `main` without parameters. Next step is declaration of variables `a`, `b` and `c` with type `photo` and assignment of string literal with full path with name. After that call switch-case statement, in which `c` variable is checked for equality with other photo. If variable is equal in one of the cases, execute function `print` to output information about photo to standart output stream. After creating execute function `main`.

**Output:**
```
functional print "test1.png"
```
