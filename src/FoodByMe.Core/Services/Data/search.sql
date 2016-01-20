SELECT Recipe.Id, Recipe.Recipe, 
MIN(CASE RecipeTextField.Type
    WHEN 2 THEN 0
    WHEN 0 THEN 1
    WHEN 1 THEN 2
    WHEN 3 THEN 4
    WHEN 5 THEN 6
	ELSE 7
 END) AS Priority
FROM RecipeTextField
JOIN Recipe ON Recipe.Id = RecipeTextField.RecipeId
JOIN RecipeTextSearch ON RecipeTextSearch.docid = RecipeTextField.Id
WHERE (RecipeTextField.Type = 0 AND RecipeTextField.Value = '3')
GROUP BY Recipe.Id
ORDER BY Priority