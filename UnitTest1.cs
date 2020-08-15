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
        List<V�rdNiv�Typ> vardNiva = new List<V�rdNiv�Typ>();
        List<V�rdkontakt> vardkontakt;

        [OneTimeSetUp]
        public void Setup()
        {
            vardNiva.Add(V�rdNiv�Typ.L�nssjukv�rd);
            vardNiva.Add(V�rdNiv�Typ.Regionsjukv�rd);
            vardNiva.Add(V�rdNiv�Typ.Rikssjukv�rd);

            vardkontakt = InitData();
        }

        private static List<V�rdkontakt> InitData()
        {
            return new List<V�rdkontakt>()
            {
                new V�rdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1747-43C2-B45D-03692D27869F"),
                    V�rdgivareId = "SU",
                    V�rdForm = V�rdFormTyp.Sluten,
                    V�rdNiv� = V�rdNiv�Typ.L�nssjukv�rd,
                    EkonomiskOmr�desKod = EkonomiskOmr�desKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504040800",
                    PatientLmaNummer = "",
                    PatientL�n = "14",
                    PatientKommun = "80",
                    Bes�ksDatumStart = "",
                    Bes�ksDatumSlut = new DateTime(2020, 08, 06),
                    S�rkostnader = 1000000,
                    Grundkostnader = 10000,
                    Priskod = "",
                    DrgKod = "A03A",
                    BetalarTyp = BetalarTyp.VGR
                },
                new V�rdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1747-43C2-B45D-03692D27849F"),
                    V�rdgivareId = "SU",
                    V�rdForm = V�rdFormTyp.�ppen,
                    V�rdNiv� = V�rdNiv�Typ.L�nssjukv�rd,
                    EkonomiskOmr�desKod = EkonomiskOmr�desKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504040900",
                    PatientLmaNummer = "",
                    PatientL�n = "14",
                    PatientKommun = "80",
                    Bes�ksDatumStart = "",
                    Bes�ksDatumSlut = new DateTime(2020, 08, 06),
                    S�rkostnader = 1000000,
                    Grundkostnader = 10000,
                    Priskod = "",
                    DrgKod = "A03A",
                    BetalarTyp = BetalarTyp.VGR
                },
                new V�rdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1747-43C2-B45D-03692D27860F"),
                    V�rdgivareId = "SU",
                    V�rdForm = V�rdFormTyp.Sluten,
                    V�rdNiv� = V�rdNiv�Typ.Regionsjukv�rd,
                    EkonomiskOmr�desKod = EkonomiskOmr�desKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504041000",
                    PatientLmaNummer = "",
                    PatientL�n = "14",
                    PatientKommun = "80",
                    Bes�ksDatumStart = "",
                    Bes�ksDatumSlut = new DateTime(2020, 08, 06),
                    S�rkostnader = 1000000,
                    Grundkostnader = 10000,
                    Priskod = "",
                    DrgKod = "A03A",
                    BetalarTyp = BetalarTyp.VGR
                },
                new V�rdkontakt()
                {
                    Id = Guid.Parse("3AC37E96-1722-43C2-B45D-03692D27860F"),
                    V�rdgivareId = "SU",
                    V�rdForm = V�rdFormTyp.Sluten,
                    V�rdNiv� = V�rdNiv�Typ.Rikssjukv�rd,
                    EkonomiskOmr�desKod = EkonomiskOmr�desKodTyp.Somatik,
                    BetalarKod = "0000",
                    PatientPersonnummer = "199504041111",
                    PatientLmaNummer = "",
                    PatientL�n = "14",
                    PatientKommun = "80",
                    Bes�ksDatumStart = "",
                    Bes�ksDatumSlut = new DateTime(2020, 08, 06),
                    S�rkostnader = 1000000,
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
                .ConditionWhere(() => vardNiva.Contains(V�rdNiv�Typ.L�nssjukv�rd),
                    f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd)
                    .ConditionWhere(() => vardNiva.Contains(V�rdNiv�Typ.Regionsjukv�rd),
                        f => f.V�rdNiv� == V�rdNiv�Typ.Regionsjukv�rd)
                        .ConditionWhere(() => vardNiva.Contains(V�rdNiv�Typ.Rikssjukv�rd),
                            f => f.V�rdNiv� == V�rdNiv�Typ.Rikssjukv�rd);

            var query2 = vardkontakt.AsQueryable()
                .ConditionalWhere(() => vardNiva.Contains(V�rdNiv�Typ.L�nssjukv�rd),
                    f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);
            
            //adding this 2nd conditionalwhere applies Where().Where() and this is not what i want right now
            //further investigation about this!
                //.ConditionalWhere(() => vardNiva.Contains(V�rdNiv�Typ.Regionsjukv�rd),
                //    f => f.V�rdNiv� == V�rdNiv�Typ.Regionsjukv�rd);
                //.ConditionalWhere(() => vardNiva.Contains(V�rdNiv�Typ.Rikssjukv�rd),
                //    f => f.V�rdNiv� == V�rdNiv�Typ.Rikssjukv�rd);

            var result = query.ToList();
            var result2 = query2.ToList();

            Assert.True(result.Count == 0);
            Assert.True(result2.Count == 2);
        }

        [Test]
        public void Test2()
        {
            var predicate = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(2, sut.Count());
        }

        [Test]
        public void Test3()
        {
            var predicate = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.Regionsjukv�rd);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(3, sut.Count());
        }

        [Test]
        public void Test4()
        {
            var predicate = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.Regionsjukv�rd);
            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.Rikssjukv�rd);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(4, sut.Count());

            /*Lambda Expression
             *.Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>(VepPrototype.Models.V�rdkontakt $f) {
                   (System.Int32)$f.V�rdNiv� == 1 || (System.Int32)$f.V�rdNiv� == 2 || (System.Int32)$f.V�rdNiv� == 3
                    }
             */
        }

        [Test]
        public void Test_False_OrElse_Lanssjukvard()
        {
            var predicate = PredicateBuilder.False<V�rdkontakt>();

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);
            //predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.Regionsjukv�rd);
            //predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.Rikssjukv�rd);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(2, sut.Count());
        }



        [Test]
        public void Test_False_OrElse_Lanssjukvard_AND_vardForm_oppen()
        {
            var predicate = PredicateBuilder.False<V�rdkontakt>();

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            predicate = predicate.And(f => f.V�rdForm == V�rdFormTyp.�ppen);


            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(1, sut.Count());

            /*
             *.Call System.Linq.Queryable.Where(
                    .Constant<System.Linq.EnumerableQuery`1[VepPrototype.Models.V�rdkontakt]>(System.Collections.Generic.List`1[VepPrototype.Models.V�rdkontakt]),
                    '(.Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>))

                .Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>(VepPrototype.Models.V�rdkontakt $param) {

                    (False || (System.Int32)$param.V�rdNiv� == 1) && (System.Int32)$param.V�rdForm == 2
                }
             */
        }


        [Test]
        public void Test_False_OrElse_Lanssjukvard_AND_vardForm_oppen_AND_EkonomiskOmradeKod_Psykiatri()
        {
            var predicate = PredicateBuilder.False<V�rdkontakt>();

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            predicate = predicate.And(f => f.V�rdForm == V�rdFormTyp.�ppen);

            predicate = predicate.And(f => f.EkonomiskOmr�desKod == EkonomiskOmr�desKodTyp.Psykiatri);


            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(0, sut.Count());

            /*
             .Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>(VepPrototype.Models.V�rdkontakt $param) {
                (False || (System.Int32)$param.V�rdNiv� == 1) && (System.Int32)$param.V�rdForm == 2 && (System.Int32)$param.EkonomiskOmr�desKod == 2
            }
             */
        }


        [Test]
        public void Test_False_OrElse_Lanssjukvard_AND_vardForm_oppen_OrElse_sluten_AND_EkonomiskOmradeKod_Psykiatri()
        {
            var predicate = PredicateBuilder.False<V�rdkontakt>();

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            predicate = predicate.And(f => f.V�rdForm == V�rdFormTyp.�ppen);
            predicate = predicate.Or(f => f.V�rdForm == V�rdFormTyp.Sluten);

            predicate = predicate.And(f => f.EkonomiskOmr�desKod == EkonomiskOmr�desKodTyp.Psykiatri);

            var sut = vardkontakt.AsQueryable().Where(predicate);

            Assert.AreEqual(0, sut.Count());

            /*
             .Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>(VepPrototype.Models.V�rdkontakt $param) {
                    (
                        (False || (System.Int32)$param.V�rdNiv� == 1) && 
                        (System.Int32)$param.V�rdForm == 2 || (System.Int32)$param.V�rdForm == 1) && 
                        (System.Int32)$param.EkonomiskOmr�desKod == 2
                }
             *
             *
             */
        }

        [Test]
        public void Test_All_Expressions_with_parentheses()
        {
            var predicate = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.Regionsjukv�rd);

            var predicate2 = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdForm == V�rdFormTyp.�ppen);

            predicate2 = predicate2.Or(f => f.V�rdForm == V�rdFormTyp.Sluten);

            var predicate3 =
                PredicateBuilder.Create<V�rdkontakt>(f => f.EkonomiskOmr�desKod == EkonomiskOmr�desKodTyp.Psykiatri);

            predicate3 = predicate3.Or(f => f.EkonomiskOmr�desKod == EkonomiskOmr�desKodTyp.Somatik);

            predicate = predicate.And(predicate2).And(predicate3);
            
            var sut = vardkontakt.AsQueryable().Where(predicate).ToList();

            Assert.AreEqual(3, sut.Count());
        }

        [Test]
        public void Test_DymanicLinq_IQueryable_Information_Expressions()
        {
            var predicate = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            predicate = predicate.Or(f => f.V�rdNiv� == V�rdNiv�Typ.Regionsjukv�rd);

            var predicate2 = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdForm == V�rdFormTyp.�ppen);

            predicate2 = predicate2.Or(f => f.V�rdForm == V�rdFormTyp.Sluten);

            var predicate3 =
                PredicateBuilder.Create<V�rdkontakt>(f => f.EkonomiskOmr�desKod == EkonomiskOmr�desKodTyp.Psykiatri);

            predicate3 = predicate3.Or(f => f.EkonomiskOmr�desKod == EkonomiskOmr�desKodTyp.Somatik);

            predicate = predicate.And(predicate2).And(predicate3);

            var predicateToString = predicate.ToString();
            //f => ((((Convert(f.V�rdNiv�, Int32) == 1) OrElse (Convert(f.V�rdNiv�, Int32) == 2)) AndAlso ((Convert(f.V�rdForm, Int32) == 2) OrElse (Convert(f.V�rdForm, Int32) == 1))) AndAlso ((Convert(f.EkonomiskOmr�desKod, Int32) == 2) OrElse (Convert(f.EkonomiskOmr�desKod, Int32) == 1)))

            Func<V�rdkontakt, bool> predicateCompiled = predicate2.Compile(); //transform it to Func

            var queryExpression = vardkontakt.AsQueryable().Where(predicate).Expression;
            var queryToString = vardkontakt.AsQueryable().Where(predicate).ToString();

            //WHERE <- queryExpression variable
            /*
             *.Call System.Linq.Queryable.Where(
                    .Constant<System.Linq.EnumerableQuery`1[VepPrototype.Models.V�rdkontakt]>(System.Collections.Generic.List`1[VepPrototype.Models.V�rdkontakt]),
                    '(.Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>))

                .Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>(VepPrototype.Models.V�rdkontakt $f) {
                    ((System.Int32)$f.V�rdNiv� == 1 || (System.Int32)$f.V�rdNiv� == 2) && ((System.Int32)$f.V�rdForm == 2 || (System.Int32)$f.V�rdForm ==
                    1) && ((System.Int32)$f.EkonomiskOmr�desKod == 2 || (System.Int32)$f.EkonomiskOmr�desKod == 1)
                }
             *
             */

            /* queryToString variable
             * System.Collections.Generic.List`1[VepPrototype.Models.V�rdkontakt]
             * .Where(f => ((((Convert(f.V�rdNiv�, Int32) == 1) OrElse (Convert(f.V�rdNiv�, Int32) == 2)) AndAlso ((Convert(f.V�rdForm, Int32) == 2) OrElse (Convert(f.V�rdForm, Int32) == 1))) AndAlso ((Convert(f.EkonomiskOmr�desKod, Int32) == 2) OrElse (Convert(f.EkonomiskOmr�desKod, Int32) == 1))))
             */

            /*
             .Lambda #Lambda1<System.Func`2[VepPrototype.Models.V�rdkontakt,System.Boolean]>(VepPrototype.Models.V�rdkontakt $f) {
                    ((System.Int32)$f.V�rdNiv� == 1 || (System.Int32)$f.V�rdNiv� == 2) && 
                    ((System.Int32)$f.V�rdForm == 2 || (System.Int32)$f.V�rdForm == 1) && 
                    ((System.Int32)$f.EkonomiskOmr�desKod == 2 || (System.Int32)$f.EkonomiskOmr�desKod == 1)
            }
             */
        }
        
        [Test]
        public void Test_DymanicLinq_Expression1()
        {

            var sut = vardkontakt.AsQueryable()
                .Where("EkonomiskOmr�desKod == @0", EkonomiskOmr�desKodTyp.Psykiatri);

            Assert.AreEqual(0, sut.Count());
        }

        [Test]
        public void Test_DymanicLinq_Expression2()
        {
            var sut = vardkontakt.AsQueryable()
                .Where("EkonomiskOmr�desKod == @0", EkonomiskOmr�desKodTyp.Somatik)
                .OrderBy("PatientPersonnummer")
                .ToList();
            
            Assert.AreEqual(4, sut.Count());
            Assert.AreEqual("199504040800", sut[0].PatientPersonnummer);
        }

        [Test]
        public void Test_DymanicLinq_Expression3()
        {
            var sut = vardkontakt.AsQueryable()
                .Where("EkonomiskOmr�desKod == @0", EkonomiskOmr�desKodTyp.Somatik)
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
            // Manually build the expression tree for the lambda expression f => (f == L�nssjukv�rd)

            ParameterExpression parameter = Expression.Parameter(typeof(V�rdNiv�Typ), "f");
            ConstantExpression rightHand = Expression.Constant(V�rdNiv�Typ.L�nssjukv�rd, typeof(V�rdNiv�Typ));
            BinaryExpression comparison = Expression.Equal(parameter, rightHand);
            Expression<Func<V�rdNiv�Typ, bool>> lambda =
                Expression.Lambda<Func<V�rdNiv�Typ, bool>>(
                    comparison,
                    new ParameterExpression[] { parameter });

            Assert.True(lambda.ToString() == "f => (f == L�nssjukv�rd)");
        }

        [Test]
        public void Test_DymanicLinq_Expression_Build3()
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries

            // Manually build the expression tree for the lambda expression x => x.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd

            ParameterExpression parameter = Expression.Parameter(typeof(V�rdkontakt), "x");

            Expression left = Expression.Property(parameter, typeof(V�rdkontakt).GetProperty("V�rdNiv�"));
            Expression right = Expression
                                //.Constant(V�rdNiv�Typ.L�nssjukv�rd);
                                .Constant(V�rdNiv�Typ.L�nssjukv�rd, typeof(V�rdNiv�Typ));
            Expression comparison = Expression.Equal(left, right);
            //-		comparison	{(x.V�rdNiv� == L�nssjukv�rd)}	System.Linq.Expressions.Expression {System.Linq.Expressions.LogicalBinaryExpression}

            //Expression<Func<V�rdNiv�Typ, bool>> lambda =
            //    Expression.Lambda<Func<V�rdNiv�Typ, bool>>(
            //        comparison,
            //        new ParameterExpression[] { parameter });


            var queryableData = vardkontakt.AsQueryable();


            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                Expression.Lambda<Func<V�rdkontakt, bool>>(comparison, new ParameterExpression[] { parameter }));

            //[1]	{x => (x.V�rdNiv� == L�nssjukv�rd)}	System.Linq.Expressions.Expression {System.Linq.Expressions.UnaryExpression}
            var lambdaExpression = whereCallExpression.Arguments[1].ToString();

            var filteredResult = queryableData.Provider.CreateQuery<V�rdkontakt>(whereCallExpression);

            //var result = queryableData.Where(whereCallExpression)
            Assert.True(lambdaExpression == "x => (x.V�rdNiv� == L�nssjukv�rd)");
            Assert.True(filteredResult.Count() == 2);
        }


        [Test]
        public void Test_DymanicLinq_Expression_Build4()
        {
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries

            // Manually build the expression tree for the lambda expression
            // x => x.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd && x.V�rdForm == V�rdFormTyp.Sluten

            ParameterExpression parameter = Expression.Parameter(typeof(V�rdkontakt), "x");

            Expression left = Expression.Property(parameter, typeof(V�rdkontakt).GetProperty("V�rdNiv�"));
            Expression right = Expression.Constant(V�rdNiv�Typ.L�nssjukv�rd, typeof(V�rdNiv�Typ));
            Expression comparison = Expression.Equal(left, right);
            //-		comparison	{(x.V�rdNiv� == L�nssjukv�rd)}	System.Linq.Expressions.Expression {System.Linq.Expressions.LogicalBinaryExpression}

            Expression left2 = Expression.Property(parameter, typeof(V�rdkontakt).GetProperty("V�rdForm"));
            Expression right2 = Expression.Constant(V�rdFormTyp.Sluten, typeof(V�rdFormTyp));
            Expression comparison2 = Expression.Equal(left2, right2);
            //comparison2	{(x.V�rdForm == Sluten)}	System.Linq.Expressions.Expression {System.Linq.Expressions.LogicalBinaryExpression}

            Expression queryToAsk = Expression.And(comparison, comparison2);
            //queryToAsk	{((x.V�rdNiv� == L�nssjukv�rd) And (x.V�rdForm == Sluten))}	System.Linq.Expressions.Expression {System.Linq.Expressions.SimpleBinaryExpression}

            var queryableData = vardkontakt.AsQueryable();
            
            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryableData.ElementType },
                queryableData.Expression,
                Expression.Lambda<Func<V�rdkontakt, bool>>(queryToAsk, new ParameterExpression[] { parameter }));
            
            var lambdaExpression = whereCallExpression.Arguments[1].ToString();
            //x => ((x.V�rdNiv� == L�nssjukv�rd) And(x.V�rdForm == Sluten))

            var filteredResult = queryableData.Provider.CreateQuery<V�rdkontakt>(whereCallExpression);
            
            Assert.True(filteredResult.Count() == 1);

            
        }

        [Test]
        public void Test_SprintFilter_Expression_Build5()
        {
            //var stored1 = "x => x.V�rdForm == Sluten";
            //var stored = "x => ((x.V�rdNiv� == L�nssjukv�rd) And(x.V�rdForm == Sluten))";
            //var predicate = PredicateBuilder.Create<V�rdkontakt>(f => f.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd);

            var serialized = Filter.Serialize<V�rdkontakt>(x => 
                x.V�rdForm == V�rdFormTyp.�ppen &&
                x.V�rdNiv� == V�rdNiv�Typ.L�nssjukv�rd &&
                x.EkonomiskOmr�desKod == EkonomiskOmr�desKodTyp.Psykiatri
                );

            var deserializedPredicate = Filter.Deserialize<V�rdkontakt>(serialized);

            var sut = vardkontakt.AsQueryable().Where(deserializedPredicate);

            Assert.AreEqual(0, sut.Count());
            
        }


    }
}