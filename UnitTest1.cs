using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using NUnit.Framework;
using DynamicExpressions;
using Sprint.Filter.OData;
using VepPrototype.Models;


namespace VepPrototype.DynamicLinq
{
    public class Tests
    {
        List<VårdNivåTyp> vardNiva = new List<VårdNivåTyp>();
        List<Vårdkontakt> vardkontakt;

        [OneTimeSetUp]
        public void Setup()
        {
            vardNiva.Add(VårdNivåTyp.Länssjukvård);
            vardNiva.Add(VårdNivåTyp.Regionsjukvård);
            vardNiva.Add(VårdNivåTyp.Rikssjukvård);

            vardkontakt = InitData();
        }

        private static List<Vårdkontakt> InitData()
        {
            return new List<Vårdkontakt>()
            {
                new Vårdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1747-43C2-B45D-03692D27869F"),
                    VårdgivareId = "SU",
                    VårdForm = VårdFormTyp.Sluten,
                    VårdNivå = VårdNivåTyp.Länssjukvård,
                    EkonomiskOmrådesKod = EkonomiskOmrådesKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504040800",
                    PatientLmaNummer = "",
                    PatientLän = "14",
                    PatientKommun = "80",
                    BesöksDatumStart = "",
                    BesöksDatumSlut = new DateTime(2020, 08, 06),
                    Särkostnader = 1000000,
                    Grundkostnader = 10000,
                    Priskod = "",
                    DrgKod = "A03A",
                    BetalarTyp = BetalarTyp.VGR
                },
                new Vårdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1747-43C2-B45D-03692D27849F"),
                    VårdgivareId = "SU",
                    VårdForm = VårdFormTyp.Öppen,
                    VårdNivå = VårdNivåTyp.Länssjukvård,
                    EkonomiskOmrådesKod = EkonomiskOmrådesKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504040900",
                    PatientLmaNummer = "",
                    PatientLän = "14",
                    PatientKommun = "80",
                    BesöksDatumStart = "",
                    BesöksDatumSlut = new DateTime(2020, 08, 06),
                    Särkostnader = 1000000,
                    Grundkostnader = 10000,
                    Priskod = "",
                    DrgKod = "A03A",
                    BetalarTyp = BetalarTyp.VGR
                },
                new Vårdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1747-43C2-B45D-03692D27860F"),
                    VårdgivareId = "SU",
                    VårdForm = VårdFormTyp.Sluten,
                    VårdNivå = VårdNivåTyp.Regionsjukvård,
                    EkonomiskOmrådesKod = EkonomiskOmrådesKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504041000",
                    PatientLmaNummer = "",
                    PatientLän = "14",
                    PatientKommun = "80",
                    BesöksDatumStart = "",
                    BesöksDatumSlut = new DateTime(2020, 08, 06),
                    Särkostnader = 1000000,
                    Grundkostnader = 10000,
                    Priskod = "",
                    DrgKod = "A03A",
                    BetalarTyp = BetalarTyp.VGR
                },
                new Vårdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1722-43C2-B45D-03692D27860F"),
                    VårdgivareId = "SU",
                    VårdForm = VårdFormTyp.Sluten,
                    VårdNivå = VårdNivåTyp.Rikssjukvård,
                    EkonomiskOmrådesKod = EkonomiskOmrådesKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504041111",
                    PatientLmaNummer = "",
                    PatientLän = "14",
                    PatientKommun = "80",
                    BesöksDatumStart = "",
                    BesöksDatumSlut = new DateTime(2020, 08, 06),
                    Särkostnader = 1000000,
                    Grundkostnader = 10000,
                    Priskod = "",
                    DrgKod = "A03A",
                    BetalarTyp = BetalarTyp.VGR
                }
            };
        }

        [Test]
        public void Test1_Applying_Where_Concatenation()
        {
            var query = vardkontakt
                .ConditionWhere(() => vardNiva.Contains(VårdNivåTyp.Länssjukvård),
                    f => f.VårdNivå == VårdNivåTyp.Länssjukvård)
                    .ConditionWhere(() => vardNiva.Contains(VårdNivåTyp.Regionsjukvård),
                        f => f.VårdNivå == VårdNivåTyp.Regionsjukvård)
                        .ConditionWhere(() => vardNiva.Contains(VårdNivåTyp.Rikssjukvård),
                            f => f.VårdNivå == VårdNivåTyp.Rikssjukvård);

            var query2 = vardkontakt.AsQueryable()
                .ConditionalWhere(() => vardNiva.Contains(VårdNivåTyp.Länssjukvård),
                    f => f.VårdNivå == VårdNivåTyp.Länssjukvård);
            
            //adding this 2nd conditionalwhere applies Where().Where() and this is not what i want right now
            //further investigation about this!
                //.ConditionalWhere(() => vardNiva.Contains(VårdNivåTyp.Regionsjukvård),
                //    f => f.VårdNivå == VårdNivåTyp.Regionsjukvård);
                //.ConditionalWhere(() => vardNiva.Contains(VårdNivåTyp.Rikssjukvård),
                //    f => f.VårdNivå == VårdNivåTyp.Rikssjukvård);

            var result = query.ToList();
            var result2 = query2.ToList();

            Assert.True(result.Count == 0);
            Assert.True(result2.Count == 2);
        }

        [Test]
        public void Test2()
        {
            var predicate = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(2, sut.Count());
        }

        [Test]
        public void Test3()
        {
            var predicate = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Regionsjukvård);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(3, sut.Count());
        }

        [Test]
        public void Test4()
        {
            var predicate = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Regionsjukvård);
            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Rikssjukvård);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(4, sut.Count());

            /*Lambda Expression
             *.Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>(VepPrototype.Models.Vårdkontakt $f) {
                   (System.Int32)$f.VårdNivå == 1 || (System.Int32)$f.VårdNivå == 2 || (System.Int32)$f.VårdNivå == 3
                    }
             */
        }

        [Test]
        public void Test_False_OrElse_Lanssjukvard()
        {
            var predicate = PredicateBuilder.False<Vårdkontakt>();

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);
            //predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Regionsjukvård);
            //predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Rikssjukvård);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(2, sut.Count());
        }



        [Test]
        public void Test_False_OrElse_Lanssjukvard_AND_vardForm_oppen()
        {
            var predicate = PredicateBuilder.False<Vårdkontakt>();

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            predicate = predicate.And(f => f.VårdForm == VårdFormTyp.Öppen);


            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(1, sut.Count());

            /*
             *.Call System.Linq.Queryable.Where(
                    .Constant<System.Linq.EnumerableQuery`1[VepPrototype.Models.Vårdkontakt]>(System.Collections.Generic.List`1[VepPrototype.Models.Vårdkontakt]),
                    '(.Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>))

                .Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>(VepPrototype.Models.Vårdkontakt $param) {

                    (False || (System.Int32)$param.VårdNivå == 1) && (System.Int32)$param.VårdForm == 2
                }
             */
        }


        [Test]
        public void Test_False_OrElse_Lanssjukvard_AND_vardForm_oppen_AND_EkonomiskOmradeKod_Psykiatri()
        {
            var predicate = PredicateBuilder.False<Vårdkontakt>();

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            predicate = predicate.And(f => f.VårdForm == VårdFormTyp.Öppen);

            predicate = predicate.And(f => f.EkonomiskOmrådesKod == EkonomiskOmrådesKodTyp.Psykiatri);


            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(0, sut.Count());

            /*
             .Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>(VepPrototype.Models.Vårdkontakt $param) {
                (False || (System.Int32)$param.VårdNivå == 1) && (System.Int32)$param.VårdForm == 2 && (System.Int32)$param.EkonomiskOmrådesKod == 2
            }
             */
        }


        [Test]
        public void Test_False_OrElse_Lanssjukvard_AND_vardForm_oppen_OrElse_sluten_AND_EkonomiskOmradeKod_Psykiatri()
        {
            var predicate = PredicateBuilder.False<Vårdkontakt>();

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            predicate = predicate.And(f => f.VårdForm == VårdFormTyp.Öppen);
            predicate = predicate.Or(f => f.VårdForm == VårdFormTyp.Sluten);

            predicate = predicate.And(f => f.EkonomiskOmrådesKod == EkonomiskOmrådesKodTyp.Psykiatri);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(0, sut.Count());

            /*
             .Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>(VepPrototype.Models.Vårdkontakt $param) {
                    (
                        (False || (System.Int32)$param.VårdNivå == 1) && 
                        (System.Int32)$param.VårdForm == 2 || (System.Int32)$param.VårdForm == 1) && 
                        (System.Int32)$param.EkonomiskOmrådesKod == 2
                }
             *
             *
             */
        }

        [Test]
        public void Test_All_Expressions_with_parentheses()
        {
            var predicate = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Regionsjukvård);

            var predicate2 = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdForm == VårdFormTyp.Öppen);

            predicate2 = predicate2.Or(f => f.VårdForm == VårdFormTyp.Sluten);

            var predicate3 =
                PredicateBuilder.Create<Vårdkontakt>(f => f.EkonomiskOmrådesKod == EkonomiskOmrådesKodTyp.Psykiatri);

            predicate3 = predicate3.Or(f => f.EkonomiskOmrådesKod == EkonomiskOmrådesKodTyp.Somatik);

            predicate = predicate.And(predicate2).And(predicate3);
            
            var sut = vardkontakt.AsQueryable().Where(predicate).ToList();

            Assert.AreEqual(3, sut.Count());
        }

        [Test]
        public void Test_DymanicLinq_IQueryable_Information_Expressions()
        {
            var predicate = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            predicate = predicate.Or(f => f.VårdNivå == VårdNivåTyp.Regionsjukvård);

            var predicate2 = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdForm == VårdFormTyp.Öppen);

            predicate2 = predicate2.Or(f => f.VårdForm == VårdFormTyp.Sluten);

            var predicate3 =
                PredicateBuilder.Create<Vårdkontakt>(f => f.EkonomiskOmrådesKod == EkonomiskOmrådesKodTyp.Psykiatri);

            predicate3 = predicate3.Or(f => f.EkonomiskOmrådesKod == EkonomiskOmrådesKodTyp.Somatik);

            predicate = predicate.And(predicate2).And(predicate3);

            var predicateToString = predicate.ToString();
            //f => ((((Convert(f.VårdNivå, Int32) == 1) OrElse (Convert(f.VårdNivå, Int32) == 2)) AndAlso ((Convert(f.VårdForm, Int32) == 2) OrElse (Convert(f.VårdForm, Int32) == 1))) AndAlso ((Convert(f.EkonomiskOmrådesKod, Int32) == 2) OrElse (Convert(f.EkonomiskOmrådesKod, Int32) == 1)))

            Func<Vårdkontakt, bool> predicateCompiled = predicate2.Compile(); //transform it to Func

            var queryExpression = vardkontakt.AsQueryable().Where(predicate).Expression;
            var queryToString = vardkontakt.AsQueryable().Where(predicate).ToString();

            //WHERE <- queryExpression variable
            /*
             *.Call System.Linq.Queryable.Where(
                    .Constant<System.Linq.EnumerableQuery`1[VepPrototype.Models.Vårdkontakt]>(System.Collections.Generic.List`1[VepPrototype.Models.Vårdkontakt]),
                    '(.Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>))

                .Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>(VepPrototype.Models.Vårdkontakt $f) {
                    ((System.Int32)$f.VårdNivå == 1 || (System.Int32)$f.VårdNivå == 2) && ((System.Int32)$f.VårdForm == 2 || (System.Int32)$f.VårdForm ==
                    1) && ((System.Int32)$f.EkonomiskOmrådesKod == 2 || (System.Int32)$f.EkonomiskOmrådesKod == 1)
                }
             *
             */

            /* queryToString variable
             * System.Collections.Generic.List`1[VepPrototype.Models.Vårdkontakt]
             * .Where(f => ((((Convert(f.VårdNivå, Int32) == 1) OrElse (Convert(f.VårdNivå, Int32) == 2)) AndAlso ((Convert(f.VårdForm, Int32) == 2) OrElse (Convert(f.VårdForm, Int32) == 1))) AndAlso ((Convert(f.EkonomiskOmrådesKod, Int32) == 2) OrElse (Convert(f.EkonomiskOmrådesKod, Int32) == 1))))
             */

            /*
             .Lambda #Lambda1<System.Func`2[VepPrototype.Models.Vårdkontakt,System.Boolean]>(VepPrototype.Models.Vårdkontakt $f) {
                    ((System.Int32)$f.VårdNivå == 1 || (System.Int32)$f.VårdNivå == 2) && 
                    ((System.Int32)$f.VårdForm == 2 || (System.Int32)$f.VårdForm == 1) && 
                    ((System.Int32)$f.EkonomiskOmrådesKod == 2 || (System.Int32)$f.EkonomiskOmrådesKod == 1)
            }
             */
        }
        
        [Test]
        public void Test_DymanicLinq_Expression1()
        {

            var sut = vardkontakt.AsQueryable()
                .Where("EkonomiskOmrådesKod == @0", EkonomiskOmrådesKodTyp.Psykiatri);

            Assert.AreEqual(0, sut.Count());
        }

        [Test]
        public void Test_DymanicLinq_Expression2()
        {
            var sut = vardkontakt.AsQueryable()
                .Where("EkonomiskOmrådesKod == @0", EkonomiskOmrådesKodTyp.Somatik)
                .OrderBy("PatientPersonnummer")
                .ToList();
            
            Assert.AreEqual(4, sut.Count());
            Assert.AreEqual("199504040800", sut[0].PatientPersonnummer);
        }

        [Test]
        public void Test_DymanicLinq_Expression3()
        {
            var sut = vardkontakt.AsQueryable()
                .Where("EkonomiskOmrådesKod == @0", EkonomiskOmrådesKodTyp.Somatik)
                .OrderBy("PatientPersonnummer")
                .Select("new(PatientPersonnummer as Personnr, DrgKod)");
            
            Assert.AreEqual(4, sut.Count());
        }

        [Test]
        public void Test_DymanicLinq_Expression_Build1()
        {
            // Manually build the expression tree for 
            // the lambda expression num => num < 5.
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
            Expression<Func<int, bool>> lambda =
                Expression.Lambda<Func<int, bool>>(
                    numLessThanFive,
                    new ParameterExpression[] { numParam });

            Assert.True(lambda.ToString() == "num => (num < 5)");
        }

        [Test]
        public void Test_DymanicLinq_Expression_Build2()
        {
            // Manually build the expression tree for the lambda expression f => (f == Länssjukvård)

            ParameterExpression parameter = Expression.Parameter(typeof(VårdNivåTyp), "f");
            ConstantExpression rightHand = Expression.Constant(VårdNivåTyp.Länssjukvård, typeof(VårdNivåTyp));
            BinaryExpression comparison = Expression.Equal(parameter, rightHand);
            Expression<Func<VårdNivåTyp, bool>> lambda =
                Expression.Lambda<Func<VårdNivåTyp, bool>>(
                    comparison,
                    new ParameterExpression[] { parameter });

            Assert.True(lambda.ToString() == "f => (f == Länssjukvård)");
        }

        [Test]
        public void Test_DymanicLinq_Expression_Build3()
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries

            // Manually build the expression tree for the lambda expression x => x.VårdNivå == VårdNivåTyp.Länssjukvård

            ParameterExpression parameter = Expression.Parameter(typeof(Vårdkontakt), "x");

            Expression left = Expression.Property(parameter, typeof(Vårdkontakt).GetProperty("VårdNivå"));
            Expression right = Expression
                                //.Constant(VårdNivåTyp.Länssjukvård);
                                .Constant(VårdNivåTyp.Länssjukvård, typeof(VårdNivåTyp));
            Expression comparison = Expression.Equal(left, right);
            //-		comparison	{(x.VårdNivå == Länssjukvård)}	System.Linq.Expressions.Expression {System.Linq.Expressions.LogicalBinaryExpression}

            //Expression<Func<VårdNivåTyp, bool>> lambda =
            //    Expression.Lambda<Func<VårdNivåTyp, bool>>(
            //        comparison,
            //        new ParameterExpression[] { parameter });


            var queryableData = vardkontakt.AsQueryable();


            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                Expression.Lambda<Func<Vårdkontakt, bool>>(comparison, new ParameterExpression[] { parameter }));

            //[1]	{x => (x.VårdNivå == Länssjukvård)}	System.Linq.Expressions.Expression {System.Linq.Expressions.UnaryExpression}
            var lambdaExpression = whereCallExpression.Arguments[1].ToString();

            var filteredResult = queryableData.Provider.CreateQuery<Vårdkontakt>(whereCallExpression);

            //var result = queryableData.Where(whereCallExpression)
            Assert.True(lambdaExpression == "x => (x.VårdNivå == Länssjukvård)");
            Assert.True(filteredResult.Count() == 2);
        }


        [Test]
        public void Test_DymanicLinq_Expression_Build4()
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries

            // Manually build the expression tree for the lambda expression
            // x => x.VårdNivå == VårdNivåTyp.Länssjukvård && x.VårdForm == VårdFormTyp.Sluten

            ParameterExpression parameter = Expression.Parameter(typeof(Vårdkontakt), "x");

            Expression left = Expression.Property(parameter, typeof(Vårdkontakt).GetProperty("VårdNivå"));
            Expression right = Expression.Constant(VårdNivåTyp.Länssjukvård, typeof(VårdNivåTyp));
            Expression comparison = Expression.Equal(left, right);
            //-		comparison	{(x.VårdNivå == Länssjukvård)}	System.Linq.Expressions.Expression {System.Linq.Expressions.LogicalBinaryExpression}

            Expression left2 = Expression.Property(parameter, typeof(Vårdkontakt).GetProperty("VårdForm"));
            Expression right2 = Expression.Constant(VårdFormTyp.Sluten, typeof(VårdFormTyp));
            Expression comparison2 = Expression.Equal(left2, right2);
            //comparison2	{(x.VårdForm == Sluten)}	System.Linq.Expressions.Expression {System.Linq.Expressions.LogicalBinaryExpression}

            Expression queryToAsk = Expression.And(comparison, comparison2);
            //queryToAsk	{((x.VårdNivå == Länssjukvård) And (x.VårdForm == Sluten))}	System.Linq.Expressions.Expression {System.Linq.Expressions.SimpleBinaryExpression}

            var queryableData = vardkontakt.AsQueryable();
            
            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                Expression.Lambda<Func<Vårdkontakt, bool>>(queryToAsk, new ParameterExpression[] { parameter }));
            
            var lambdaExpression = whereCallExpression.Arguments[1].ToString();
            //x => ((x.VårdNivå == Länssjukvård) And(x.VårdForm == Sluten))

            var filteredResult = queryableData.Provider.CreateQuery<Vårdkontakt>(whereCallExpression);
            
            Assert.True(filteredResult.Count() == 1);

            
        }

        [Test]
        public void Test_SprintFilter_Expression_Build5()
        {
            //var stored1 = "x => x.VårdForm == Sluten";
            //var stored = "x => ((x.VårdNivå == Länssjukvård) And(x.VårdForm == Sluten))";
            //var predicate = PredicateBuilder.Create<Vårdkontakt>(f => f.VårdNivå == VårdNivåTyp.Länssjukvård);

            var serialized = Filter.Serialize<Vårdkontakt>(x => 
                x.VårdForm == VårdFormTyp.Öppen &&
                x.VårdNivå == VårdNivåTyp.Länssjukvård &&
                x.EkonomiskOmrådesKod == EkonomiskOmrådesKodTyp.Psykiatri
                );

            var deserializedPredicate = Filter.Deserialize<Vårdkontakt>(serialized);

            var sut = vardkontakt.AsQueryable().Where(deserializedPredicate);

            Assert.AreEqual(0, sut.Count());
            
        }


    }
}