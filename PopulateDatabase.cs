// [HttpGet("PopulateDatabaseTemplate")]
// public async Task<ActionResult> Populate()
// {
//     var apiUrl = "https://random-word-api.herokuapp.com/word?number=5000";
//
//     var httpClient = new HttpClient();
//     var response = await httpClient.GetAsync(apiUrl);
//     var words = await response.Content.ReadAsStringAsync();
//     var wordsArray = System.Text.Json.JsonSerializer.Deserialize<List<string>>(words);
//
//     string[] types = ["Unit", "Assembly", "Subassembly", "Part"];
//     var rnd = new Random();
//
//     for (var i = 0; i < 100; i++)
//     {
//         var template = new ItemTemplate
//         {
//             Name = "Template" + (i+1),
//             Type = types[rnd.Next(types.Length)],
//             ProductNumber = "ProductNumber" + (i+1),
//             Description = "",
//             CreatedDate = DateTime.Now
//         };
//             
//         for (var k = 0; k < 30; k++)
//         {
//             template.Description += (wordsArray[rnd.Next(wordsArray.Count)] + " ");
//         }
//             
//         await _context.ItemTemplates.AddAsync(template);
//     }
//
//     await _context.SaveChangesAsync();
//     return Ok();
// }
//
//
// [HttpGet("PopulateDatabaseItem")]
// public async Task<ActionResult> Populate()
// {
//     var itemTemplates = await _itemTemplateService.GetAllItemTemplatesAsync();
//     var itemTemplatesList = itemTemplates.ToList();
//
//     var items = await _itemService.GetAllItemsAsync();
//     var itemsList = items.ToList();
//
//     var rnd = new Random();
//             
//     for (var i = 10001; i <= 20000; i++)
//     {
//         var item = new Item
//         {
//             WpId = "WpId" + i,
//             SerialNumber = "SerialNumber" + i,
//             ItemTemplateId = itemTemplatesList[rnd.Next(itemTemplatesList.Count)].Id,
//             ParentId = itemsList[rnd.Next(itemsList.Count)].Id
//         };
//         await _context.Items.AddAsync(item);
//     }
//     await _context.SaveChangesAsync();
//     return Ok();
// }