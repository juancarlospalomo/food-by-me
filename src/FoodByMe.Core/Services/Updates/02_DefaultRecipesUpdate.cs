using System;
using System.Collections.Generic;
using FoodByMe.Core.Contracts;
using FoodByMe.Core.Contracts.Data;
using FoodByMe.Core.Services.Data;

namespace FoodByMe.Core.Services.Updates
{
    [Update(2, "Initial set of recipes")]
    internal class DefaultRecipesUpdate : IUpdate
    {
        private static Dictionary<string, Func<IReferenceBookService, IEnumerable<Recipe>>> _recipes =
            new Dictionary<string, Func<IReferenceBookService, IEnumerable<Recipe>>>
        {
                {"ru", Russian},
                {"en", English}

        }; 

        public void Apply(UpdateContext context)
        {
            var service = context.RecipePersistenceService;
            var lang = context.CultureProvider.Culture.TwoLetterISOLanguageName;
            if (!_recipes.ContainsKey(lang))
            {
                return;
            }
            foreach (var recipe in _recipes[lang](context.ReferenceBook))
            {
                service.SaveRecipe(recipe);
            }
        }

        private static IEnumerable<Recipe> Russian(IReferenceBookService referenceBook)
        {
            return new List<Recipe>
            {
                new Recipe
                {
                    Title = "Плов с курицей",
                    Category = referenceBook.LookupCategory(Constants.Categories.Dinner),
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    CookingMinutes = 100,
                    CookingSteps = new List<string>
                    {
                        "Лук нарезать большими квадратами (чтобы он хорошо чувствовался в плове).",
                        "Морковь помыть, почистить и потереть на крупной терке.",
                        "Куриное филе порезать на небольшие кусочки.",
                        "На разогретой сковородке обжарить лук до золотисктого цвета в растительном масле.",
                        "Добавить порезанное куриное филе и слегка обжарить вместе с луком.",
                        "К жареному луку и куриному филе добавить порезанную брусочками морковь. Накрыть крушкой и " +
                        "потушить около 5 минут.",
                        "Открыть крышку и добавить специи: барбарис, зиру (кумин), куркуму, смесь перцев и соль.",
                        "Залить водой, чтобы она покрывала мясо. Тушить на небольшом огне под крышкой около 20 - 25 минут.",
                        "Теперь главное, рис промыть (4 - 5 раз под водой). Засыпать рисом мясо и, главное, НЕ перемешивать.",
                        "Последний этап - залить рис водой, чтобы она покрывала его на 1,5 - 2 см. Тушить под крышкой.",
                        "Через 15 минут берем 6 зубчиков чеснока, и не очищая его вдавливаем в рис. Снова накрываем крышкой" +
                        "и готовим до готовности риса и полного впитывания воды еще около 10 минут."
                    },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 1, Title = "Лук"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 1, Title = "Морковь"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 400, Title = "Куриное филе"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 10, Title = "Специи"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 1, Title = "Чеснок"}
                    }
                },
                new Recipe
                {
                    Title = "Запеканка",
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    Notes = "Запекать 25 минут, при температуре 170 градусов.",
                    CookingMinutes = 45,
                    Category = referenceBook.LookupCategory(Constants.Categories.Baking),
                    CookingSteps = new List<string>
                    {
                        "Смешать все ингредиенты, кроме корицы.",
                        "Корицу засыпать в форму с творожной массой."
                    },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 500, Title = "Творог"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 100, Title = "Сметана"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 5, Title = "Манка"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 10, Title = "Ванильный сахар"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 3, Title = "Сахар"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 10, Title = "Корица"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 3, Title = "Яйца"}
                    }
                },
                new Recipe
                {
                    Title = "Сэндвичи с моцареллой",
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    Notes = "Сэндвичи должны как следует подрумяниться. Разрежьте каждый на четыре части и подавайте.",
                    CookingMinutes = 20,
                    Category = referenceBook.LookupCategory(Constants.Categories.Appetizers),
                    CookingSteps = new List<string>
                    {
                        "Тонко нарежьте моцаререллу.",
                        "Яйцо взбить с молоком.",
                        "Выложить моцареллу и ветчину на 4 куска хлеба, накройте оставшимися кусками и крепко прижмите.",
                        "Прогрейте 2 см масла в глубокой сковороде так, чтобы кусочек хлеба подрумянивался за 1 минуту.",
                        "Обваляйте бутерброды в муке, а потом обмакните в яйцо. Готовьте во фритюре 30 - 45 секунд с каждой стороны."
                    },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 150, Title = "Моцарелла"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 100, Title = "Ветчина"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 8, Title = "Куски белого хлеба"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 10, Title = "Растительное масло"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 1, Title = "Муки"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Milliliter), Quantity = 125, Title = "Молока"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 1, Title = "Яйца"}
                    }
                },
                new Recipe
                {
                    Title = "Шоколадный крем с бананами",
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    Notes = "Подать крем теплым.",
                    CookingMinutes = 30,
                    Category = referenceBook.LookupCategory(Constants.Categories.Desserts),
                    CookingSteps = new List<string>
                    {
                        "Разогрейте духовку до 150 гадусов. Насыпьте какао в сотейник и залейте молоком. Помешивая, дооведите до кипения.",
                        "Смешайте в миске яйца, желток и сахар. Влейте горячий шоколад, энергично помешивая.",
                        "Разлейте смесь по 4 жаропрочным формочкам и поставьте в глубокий противаень.",
                        "Залейте противень кипятком так, чтобы вода доходила до середины формочек.",
                        "Выпекайте 15 - 18 минут. Крем должен загустеть, но остаться желеобразным.",
                        "Подавайте с кружками банана."
                    },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 3, Title = "Какао"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Milliliter), Quantity = 450, Title = "Нежирного молока"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 3, Title = "Яйца"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 1, Title = "Желток"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 65, Title = "Мелкого сахарного песка"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Piece), Quantity = 2, Title = "Банана"}
                    }
                },
                new Recipe
                {
                    Title = "Горячий кофейный шоколад",
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    Notes = "Украсьте шоколадной стружкой и подавайте. Бренди можно не добавлять.",
                    CookingMinutes = 20,
                    Category = referenceBook.LookupCategory(Constants.Categories.Drinks),
                    CookingSteps = new List<string>
                    {
                        "Взбивайте сливки с корицей и бренди, пока они слегка не загустеют.",
                        "Разведите кофе 300 мл кипятка. Подогрейте молоко в сотейнике, не доводя до кипения." +
                        "Вмешайте шоколад до полного растворения.",
                        "Разлейте кофе по 2 кружкам, затем влейте горячий шоколад. Сверху выложите сливки."
                    },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 3, Title = "Жирных  сливок"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 5, Title = "Молотой корицы"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 1, Title = "Бренди"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TeaSpoon), Quantity = 5, Title = "Растворимого кофе"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Milliliter), Quantity = 225, Title = "Молока"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 2, Title = "Растворимого горячего шоколада"}
                    }
                },
                new Recipe
                {
                    Title = "Картофельный салат с беконом",
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    CookingMinutes = 60,
                    Category = referenceBook.LookupCategory(Constants.Categories.Salads),
                    CookingSteps = new List<string>
                    {
                        "Варите картофель в кипящей подсоленной воде 20 минут до мягкости. Слейте воду и остудите картофель.",
                        "Раскалите сковороду на сильном огне. Обжаривайте бекон 4 - 5 минут, пока он не подрумянится и не станет" +
                        "хрустящим. Промокните лишний жир бумажным потоленцем.",
                        "Когда картофель остынет, нарежьте его и переложите в салатную иску. Заправьте майонезом и шнитт - луком, посолите и поперчите.",
                        "Разложите листья салата по тарелкам и выложите сверху картофельный салат. Посыпьте беконом и подавайте к столу."
                    },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 400, Title = "Молодого картофеля"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 5, Title = "Соль и молотый перец"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 75, Title = "Бекон"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 6, Title = "Майонез"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.TableSpoon), Quantity = 1, Title = "Шнитт - лука"},
                        new Ingredient {Measure = referenceBook.LookupMeasure(Constants.Measures.Gram), Quantity = 150, Title = "Салат латук"}
                    }
                }
            };
        }

        private static IEnumerable<Recipe> English(IReferenceBookService referenceBook)
        {
            return new List<Recipe>
            {
            };
        } 
    }
}
