using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AoC.Generators
{
    [Generator]
    public class GeneratorTest : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
//            context.AddSource("robTest", @"
//using System;

//namespace AoC
//{
//    public static class Test1
//    {
//        public static void SayHello()
//        {
//            Console.WriteLine(""Hello world! from a generator"");
//        }
//    }
//}
//");
            var classVisitor = new ClassVisitor(context.Compilation);

            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                classVisitor.Visit(syntaxTree.GetRoot());
            }

            var buffer = new StringBuilder();

            buffer.AppendLine(@"
using System;

namespace AoC
{
    public static class Test2
    {");

            foreach (var solver in classVisitor.Classes
                .Where(cls => !cls.classSymbol.IsAbstract)
                .Where(cls => cls.classSymbol.AllInterfaces.Any(i => i.Name == "ISolver")))
            {
                buffer.AppendFormat("public const string HelloFrom{1} = \"{0}.{1}\";", solver.classSymbol.ContainingNamespace, solver.classSymbol.Name);
                buffer.AppendLine();
            }

            buffer.AppendLine(@"
    }
}");
            context.AddSource("solvers", buffer.ToString());
        }

        private class ClassVisitor : CSharpSyntaxRewriter
        {
            private readonly Compilation _compilation;

            public ClassVisitor(Compilation compilation)
            {
                _compilation = compilation;
                Classes = new List<(ClassDeclarationSyntax, INamedTypeSymbol)>();
            }

            public List<(ClassDeclarationSyntax classSyntax, INamedTypeSymbol classSymbol)> Classes { get; }

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax classSyntax)
            {
                var classSemanticModel = _compilation.GetSemanticModel(classSyntax.SyntaxTree);
                var classSymbol = classSemanticModel.GetDeclaredSymbol(classSyntax);

                if (classSymbol != null)
                {
                    Classes.Add((classSyntax, classSymbol));
                }

                return base.VisitClassDeclaration(classSyntax);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required

            // rs-todo: only temp:
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
        }
    }
}
