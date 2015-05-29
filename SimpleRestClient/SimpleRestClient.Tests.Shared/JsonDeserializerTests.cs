﻿#region License
//   Copyright 2010 John Sheehan
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using NUnit.Framework;
using System.Reflection;

namespace SimpleRestClient.Tests
{
    [TestFixture]
    public class JsonDeserializerTests
    {
        private string BASE_NAME = String.Empty;
        private const string AlternativeCulture = "pt-PT";
        private const string GuidString = "AC1FC4BC-087A-4242-B8EE-C53EBE9887A5";
        private Assembly asm;

        [SetUp]
        public void Setup()
        {
            asm = Assembly.GetExecutingAssembly();
            BASE_NAME = asm.GetName().Name + ".Shared.Resources.Data.";
        }


        [Test]
        public void Can_Deserialize_4sq_Json_With_Root_Element_Specified()
        {
            var doc = Unpack("4sq.txt");

            var json = new JsonDeserializer();
            json.RootElement = "response";

            var output = json.Deserialize<VenuesResponse>(new RestResponse { Content = doc });

            Assertions.NotEmpty(output.Groups);
        }

        [Test]
        public void Can_Deserialize_Lists_of_Simple_Types()
        {
            var doc = Unpack("jsonlists.txt");
            var json = new JsonDeserializer();

            var output = json.Deserialize<JsonLists>(new RestResponse { Content = doc });

            Assertions.NotEmpty(output.Names);
            Assertions.NotEmpty(output.Numbers);
        }

        [Test]
        public void Can_Deserialize_Simple_Generic_List_of_Simple_Types()
        {
            const string content = "{\"users\":[\"johnsheehan\",\"jagregory\",\"drusellers\",\"structuremap\"]}";
            var json = new JsonDeserializer { RootElement = "users" };

            var output = json.Deserialize<List<string>>(new RestResponse { Content = content });

            Assertions.NotEmpty(output);
        }

        [Test]
        public void Can_Deserialize_Simple_Generic_List_of_Simple_Types_With_Nulls()
        {
            const string content = "{\"users\":[\"johnsheehan\",\"jagregory\",null,\"drusellers\",\"structuremap\"]}";
            var json = new JsonDeserializer { RootElement = "users" };

            var output = json.Deserialize<List<string>>(new RestResponse { Content = content });

            Assertions.NotEmpty(output);
            Assert.AreEqual(null, output[2]);
            Assert.AreEqual(5, output.Count);
        }

        [Test]
        public void Can_Deserialize_Simple_Generic_List_Given_Item_Without_Array()
        {
            const string content = "{\"users\":\"johnsheehan\"}";
            var json = new JsonDeserializer { RootElement = "users" };

            var output = json.Deserialize<List<string>>(new RestResponse { Content = content });

            Assert.IsTrue(output.SequenceEqual(new[] { "johnsheehan" }));
        }

        [Test]
        public void Can_Deserialize_Simple_Generic_List_Given_Toplevel_Item_Without_Array()
        {
            const string content = "\"johnsheehan\"";
            var json = new JsonDeserializer();

            var output = json.Deserialize<List<string>>(new RestResponse { Content = content });

            Assert.IsTrue(output.SequenceEqual(new[] { "johnsheehan" }));
        }

        [Test]
        public void Can_Deserialize_From_Root_Element()
        {
            var doc = Unpack("sojson.txt");

            var json = new JsonDeserializer();
            json.RootElement = "User";

            var output = json.Deserialize<SOUser>(new RestResponse { Content = doc });
            Assert.AreEqual("John Sheehan", output.DisplayName);
        }

        [Test]
        public void Can_Deserialize_Generic_Members()
        {
            var doc = Unpack("GenericWithList.txt");
            var json = new JsonDeserializer();

            var output = json.Deserialize<Generic<GenericWithList<Foe>>>(new RestResponse { Content = doc });
            Assert.AreEqual("Foe sho", output.Data.Items[0].Nickname);
        }

        [Test]
        public void Can_Deserialize_List_of_Guid()
        {
            Guid ID1 = new Guid("b0e5c11f-e944-478c-aadd-753b956d0c8c");
            Guid ID2 = new Guid("809399fa-21c4-4dca-8dcd-34cb697fbca0");
            var data = new JsonObject();
            data["Ids"] = new JsonArray() { ID1, ID2 };

            var d = new JsonDeserializer();
            var response = new RestResponse { Content = data.ToString() };
            var p = d.Deserialize<GuidList>(response);

            Assert.AreEqual(2, p.Ids.Count);
            Assert.AreEqual(ID1, p.Ids[0]);
            Assert.AreEqual(ID2, p.Ids[1]);
        }

        [Test]
        public void Can_Deserialize_Generic_List_of_DateTime()
        {
            DateTime Item1 = new DateTime(2010, 2, 8, 11, 11, 11);
            DateTime Item2 = Item1.AddSeconds(12345);
            var data = new JsonObject();
            data["Items"] = new JsonArray { Item1.ToString("u"), Item2.ToString("u") };

            var d = new JsonDeserializer();
            var response = new RestResponse { Content = data.ToString() };
            var p = d.Deserialize<GenericWithList<DateTime>>(response);

            Assert.AreEqual(2, p.Items.Count);
            Assert.AreEqual(Item1, p.Items[0]);
            Assert.AreEqual(Item2, p.Items[1]);
        }

        [Test]
        public void Can_Deserialize_Null_Elements_to_Nullable_Values()
        {
            var doc = CreateJsonWithNullValues();

            var json = new JsonDeserializer();
            var output = json.Deserialize<NullableValues>(new RestResponse { Content = doc });

            Assert.IsNull(output.Id);
            Assert.IsNull(output.StartDate);
            Assert.IsNull(output.UniqueId);
        }

        [Test]
        public void Can_Deserialize_Empty_Elements_to_Nullable_Values()
        {
            var doc = CreateJsonWithEmptyValues();

            var json = new JsonDeserializer();
            var output = json.Deserialize<NullableValues>(new RestResponse { Content = doc });

            Assert.IsNull(output.Id);
            Assert.IsNull(output.StartDate);
            Assert.IsNull(output.UniqueId);
        }

        [Test]
        public void Can_Deserialize_Elements_to_Nullable_Values()
        {
            var doc = CreateJsonWithoutEmptyValues();

            var json = new JsonDeserializer();
            var output = json.Deserialize<NullableValues>(new RestResponse { Content = doc });

            Assert.IsNotNull(output.Id);
            Assert.IsNotNull(output.StartDate);
            Assert.IsNotNull(output.UniqueId);

            Assert.AreEqual(123, output.Id);
            Assert.IsNotNull(output.StartDate);
            Assert.AreEqual(
                new DateTime(2010, 2, 21, 9, 35, 00, DateTimeKind.Utc),
                output.StartDate.Value);
            Assert.AreEqual(new Guid(GuidString), output.UniqueId);
        }

        [Test]
        public void Can_Deserialize_Json_Using_DeserializeAs_Attribute()
        {
            const string content = "{\"sid\":\"asdasdasdasdasdasdasda\",\"friendlyName\":\"VeryNiceName\",\"oddballPropertyName\":\"blahblah\"}";

            var json = new JsonDeserializer { RootElement = "users" };
            var output = json.Deserialize<Oddball>(new RestResponse { Content = content });

            Assert.IsNotNull(output);
            Assert.AreEqual("blahblah", output.GoodPropertyName);
        }

        [Test]
        public void Can_Deserialize_Custom_Formatted_Date()
        {
            var culture = CultureInfo.InvariantCulture;
            var format = "dd yyyy MMM, hh:mm ss tt";
            var date = new DateTime(2010, 2, 8, 11, 11, 11);

            var formatted = new
            {
                StartDate = date.ToString(format, culture)
            };

            var data = SimpleJson.SerializeObject(formatted);
            var response = new RestResponse { Content = data };

            var json = new JsonDeserializer { DateFormat = format, Culture = culture };

            var output = json.Deserialize<PersonForJson>(response);

            Assert.AreEqual(date, output.StartDate);
        }

        [Test]
        public void Can_Deserialize_Root_Json_Array_To_List()
        {
            var data = Unpack("jsonarray.txt");
            var response = new RestResponse { Content = data };
            var json = new JsonDeserializer();
            var output = json.Deserialize<List<status>>(response);
            Assert.AreEqual(4, output.Count);
        }

        [Test]
        public void Can_Deserialize_Root_Json_Array_To_Inherited_List()
        {
            var data = Unpack("jsonarray.txt");
            var response = new RestResponse { Content = data };
            var json = new JsonDeserializer();
            var output = json.Deserialize<StatusList>(response);
            Assert.AreEqual(4, output.Count);
        }

        [Test]
        public void Can_Deserialize_Various_Enum_Values()
        {
            var data = Unpack("jsonenums.txt");
            var response = new RestResponse { Content = data };
            var json = new JsonDeserializer();
            var output = json.Deserialize<JsonEnumsTestStructure>(response);

            Assert.AreEqual(Disposition.Friendly, output.Upper);
            Assert.AreEqual(Disposition.Friendly, output.Lower);
            Assert.AreEqual(Disposition.SoSo, output.CamelCased);
            Assert.AreEqual(Disposition.SoSo, output.Underscores);
            Assert.AreEqual(Disposition.SoSo, output.LowerUnderscores);
            Assert.AreEqual(Disposition.SoSo, output.Dashes);
            Assert.AreEqual(Disposition.SoSo, output.LowerDashes);
            Assert.AreEqual(Disposition.SoSo, output.Integer);
        }

        [Test]
        public void Can_Deserialize_Various_Enum_Types()
        {
            var data = Unpack("jsonenumtypes.txt");
            var response = new RestResponse { Content = data };
            var json = new JsonDeserializer();
            var output = json.Deserialize<JsonEnumTypesTestStructure>(response);

            Assert.AreEqual(ByteEnum.EnumMin, output.ByteEnumType);
            Assert.AreEqual(SByteEnum.EnumMin, output.SByteEnumType);
            Assert.AreEqual(ShortEnum.EnumMin, output.ShortEnumType);
            Assert.AreEqual(UShortEnum.EnumMin, output.UShortEnumType);
            Assert.AreEqual(IntEnum.EnumMin, output.IntEnumType);
            Assert.AreEqual(UIntEnum.EnumMin, output.UIntEnumType);
            Assert.AreEqual(LongEnum.EnumMin, output.LongEnumType);
            Assert.AreEqual(ULongEnum.EnumMin, output.ULongEnumType);
        }

        [Test]
        public void Deserialization_Of_Undefined_Int_Value_Returns_Enum_Default()
        {
            const string data = @"{ ""Integer"" : 1024 }";
            var response = new RestResponse { Content = data };
            var json = new JsonDeserializer();
            var result = json.Deserialize<JsonEnumsTestStructure>(response);
            Assert.AreEqual(Disposition.Friendly, result.Integer);
        }

        [Test]
        public void Can_Deserialize_Guid_String_Fields()
        {
            var doc = new JsonObject();
            doc["Guid"] = GuidString;

            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc.ToString() };
            var p = d.Deserialize<PersonForJson>(response);

            Assert.AreEqual(new Guid(GuidString), p.Guid);
        }

        [Test]
        public void Can_Deserialize_Quoted_Primitive()
        {
            var doc = new JsonObject();
            doc["Age"] = "28";

            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc.ToString() };
            var p = d.Deserialize<PersonForJson>(response);

            Assert.AreEqual(28, p.Age);
        }

        [Test]
        public void Can_Deserialize_Int_to_Bool()
        {
            var doc = new JsonObject();
            doc["IsCool"] = 1;

            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc.ToString() };
            var p = d.Deserialize<PersonForJson>(response);

            Assert.IsTrue(p.IsCool);
        }

        [Test]
        public void Can_Deserialize_With_Default_Root()
        {
            var doc = CreateJson();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var p = d.Deserialize<PersonForJson>(response);

            Assert.AreEqual("John Sheehan", p.Name);
            Assert.AreEqual(new DateTime(2009, 9, 25, 0, 6, 1, DateTimeKind.Utc), p.StartDate);
            Assert.AreEqual(28, p.Age);
            Assert.AreEqual(long.MaxValue, p.BigNumber);
            Assert.AreEqual(99.9999m, p.Percent);
            Assert.AreEqual(false, p.IsCool);
            Assert.AreEqual(new Uri("http://example.com", UriKind.RelativeOrAbsolute), p.Url);
            Assert.AreEqual(new Uri("/foo/bar", UriKind.RelativeOrAbsolute), p.UrlPath);

            Assert.AreEqual(Guid.Empty, p.EmptyGuid);
            Assert.AreEqual(new Guid(GuidString), p.Guid);

            Assert.AreEqual(Order.Third, p.Order);
            Assert.AreEqual(Disposition.SoSo, p.Disposition);

            Assert.IsNotNull(p.Friends);
            Assert.AreEqual(10, p.Friends.Count);

            Assert.IsNotNull(p.BestFriend);
            Assert.AreEqual("The Fonz", p.BestFriend.Name);
            Assert.AreEqual(1952, p.BestFriend.Since);

            Assertions.NotEmpty(p.Foes);
            Assert.AreEqual("Foe 1", p.Foes["dict1"].Nickname);
            Assert.AreEqual("Foe 2", p.Foes["dict2"].Nickname);
        }

        [Test]
        public void Can_Deserialize_With_Default_Root_Alternative_Culture()
        {
            using (new CultureChange(AlternativeCulture))
            {
                Can_Deserialize_With_Default_Root();
            }
        }

        [Test]
        public void Can_Deserialize_Names_With_Underscore_Prefix()
        {
            var data = Unpack("underscore_prefix.txt");
            var response = new RestResponse { Content = data };
            var json = new JsonDeserializer();
            json.RootElement = "User";

            var output = json.Deserialize<SOUser>(response);

            Assert.AreEqual("John Sheehan", output.DisplayName);
            Assert.AreEqual(1786, output.Id);
        }

        [Test]
        public void Can_Deserialize_Names_With_Underscores_With_Default_Root()
        {
            var doc = CreateJsonWithUnderscores();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var p = d.Deserialize<PersonForJson>(response);

            Assert.AreEqual("John Sheehan", p.Name);
            Assert.AreEqual(new DateTime(2009, 9, 25, 0, 6, 1), p.StartDate);
            Assert.AreEqual(28, p.Age);
            Assert.AreEqual(long.MaxValue, p.BigNumber);
            Assert.AreEqual(99.9999m, p.Percent);
            Assert.AreEqual(false, p.IsCool);
            Assert.AreEqual(new Uri("http://example.com", UriKind.RelativeOrAbsolute), p.Url);
            Assert.AreEqual(new Uri("/foo/bar", UriKind.RelativeOrAbsolute), p.UrlPath);

            Assert.IsNotNull(p.Friends);
            Assert.AreEqual(10, p.Friends.Count);

            Assert.IsNotNull(p.BestFriend);
            Assert.AreEqual("The Fonz", p.BestFriend.Name);
            Assert.AreEqual(1952, p.BestFriend.Since);

            Assertions.NotEmpty(p.Foes);
            Assert.AreEqual("Foe 1", p.Foes["dict1"].Nickname);
            Assert.AreEqual("Foe 2", p.Foes["dict2"].Nickname);
        }

        [Test]
        public void Can_Deserialize_Names_With_Underscores_With_Default_Root_Alternative_Culture()
        {
            using (new CultureChange(AlternativeCulture))
            {
                Can_Deserialize_Names_With_Underscores_With_Default_Root();
            }
        }

        [Test]
        public void Can_Deserialize_Names_With_Dashes_With_Default_Root()
        {
            var doc = CreateJsonWithDashes();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var p = d.Deserialize<PersonForJson>(response);

            Assert.AreEqual("John Sheehan", p.Name);
            //Assert.AreEqual(new DateTime(2009, 9, 25, 0, 6, 1, DateTimeKind.Utc), p.StartDate);
            Assert.AreEqual(28, p.Age);
            Assert.AreEqual(long.MaxValue, p.BigNumber);
            Assert.AreEqual(99.9999m, p.Percent);
            Assert.AreEqual(false, p.IsCool);
            Assert.AreEqual(new Uri("http://example.com", UriKind.RelativeOrAbsolute), p.Url);
            Assert.AreEqual(new Uri("/foo/bar", UriKind.RelativeOrAbsolute), p.UrlPath);

            Assert.IsNotNull(p.Friends);
            Assert.AreEqual(10, p.Friends.Count);

            Assert.IsNotNull(p.BestFriend);
            Assert.AreEqual("The Fonz", p.BestFriend.Name);
            Assert.AreEqual(1952, p.BestFriend.Since);

            Assertions.NotEmpty(p.Foes);
            Assert.AreEqual("Foe 1", p.Foes["dict1"].Nickname);
            Assert.AreEqual("Foe 2", p.Foes["dict2"].Nickname);
        }

        [Test]
        public void Can_Deserialize_Names_With_Dashes_With_Default_Root_Alternative_Culture()
        {
            using (new CultureChange(AlternativeCulture))
            {
                Can_Deserialize_Names_With_Dashes_With_Default_Root();
            }
        }

        [Test]
        public void Ignore_Protected_Property_That_Exists_In_Data()
        {
            var doc = CreateJson();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var p = d.Deserialize<PersonForJson>(response);

            Assert.IsNull(p.IgnoreProxy);
        }

        [Test]
        public void Ignore_ReadOnly_Property_That_Exists_In_Data()
        {
            var doc = CreateJson();
            var response = new RestResponse { Content = doc };
            var d = new JsonDeserializer();
            var p = d.Deserialize<PersonForJson>(response);

            Assert.IsNull(p.ReadOnlyProxy);
        }

        [Test]
        public void Can_Deserialize_TimeSpan()
        {
            var payload = GetPayLoad<TimeSpanTestStructure>("timespans.txt");

            Assert.AreEqual(new TimeSpan(468006), payload.Tick);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 0, 125), payload.Millisecond);
            Assert.AreEqual(new TimeSpan(0, 0, 8), payload.Second);
            Assert.AreEqual(new TimeSpan(0, 55, 2), payload.Minute);
            Assert.AreEqual(new TimeSpan(21, 30, 7), payload.Hour);
            Assert.IsNull(payload.NullableWithoutValue);
            Assert.IsNotNull(payload.NullableWithValue);
            Assert.AreEqual(new TimeSpan(21, 30, 7), payload.NullableWithValue.Value);
        }

        [Test]
        public void Can_Deserialize_Iso_Json_Dates()
        {
            var doc = CreateIsoDateJson();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var bd = d.Deserialize<Birthdate>(response);

            Assert.AreEqual(new DateTime(1910, 9, 25, 9, 30, 25, DateTimeKind.Utc), bd.Value);
        }

        [Test]
        public void Can_Deserialize_Unix_Json_Dates()
        {
            var doc = CreateUnixDateJson();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var bd = d.Deserialize<Birthdate>(response);

            Assert.AreEqual(new DateTime(2011, 6, 30, 8, 15, 46, DateTimeKind.Utc), bd.Value);
        }

        [Test]
        public void Can_Deserialize_JsonNet_Dates()
        {
            var person = GetPayLoad<PersonForJson>("person.json.txt");

            Assert.AreEqual(
                new DateTime(2011, 6, 30, 8, 15, 46, 929, DateTimeKind.Utc),
                person.StartDate);
        }

        [Test]
        public void Can_Deserialize_DateTime()
        {
            var payload = GetPayLoad<DateTimeTestStructure>("datetimes.txt");

            Assert.AreEqual(
                new DateTime(2011, 6, 30, 8, 15, 46, 929, DateTimeKind.Utc),
                payload.DateTime);
        }

        [Test]
        public void Can_Deserialize_Nullable_DateTime_With_Value()
        {
            var payload = GetPayLoad<DateTimeTestStructure>("datetimes.txt");

            Assert.IsNotNull(payload.NullableDateTimeWithValue);
            Assert.AreEqual(
                new DateTime(2011, 6, 30, 8, 15, 46, 929, DateTimeKind.Utc),
                payload.NullableDateTimeWithValue.Value);
        }

        [Test]
        public void Can_Deserialize_Nullable_DateTime_With_Null()
        {
            var payload = GetPayLoad<DateTimeTestStructure>("datetimes.txt");

            Assert.IsNull(payload.NullableDateTimeWithNull);
        }

        [Test]
        public void Can_Deserialize_DateTimeOffset()
        {
            var payload = GetPayLoad<DateTimeTestStructure>("datetimes.txt");
            
            var expected = new DateTimeOffset(new DateTime(2011, 6, 30, 8, 15, 46, 929,DateTimeKind.Utc));

            Assert.AreEqual(expected, payload.DateTimeOffset);
        }

        [Test]
        public void Can_Deserialize_Iso8601DateTimeLocal()
        {
            var payload = GetPayLoad<Iso8601DateTimeTestStructure>("iso8601datetimes.txt");

            Assert.AreEqual(
                new DateTime(2012, 7, 19, 10, 23, 25, DateTimeKind.Utc),
                payload.DateTimeLocal);
        }

        [Test]
        public void Can_Deserialize_Iso8601DateTimeZulu()
        {
            var payload = GetPayLoad<Iso8601DateTimeTestStructure>("iso8601datetimes.txt");

            Assert.AreEqual(
                new DateTime(2012, 7, 19, 10, 23, 25, 544, DateTimeKind.Utc),
                payload.DateTimeUtc.ToUniversalTime());
        }

        [Test]
        public void Can_Deserialize_Iso8601DateTimeWithOffset()
        {
            var payload = GetPayLoad<Iso8601DateTimeTestStructure>("iso8601datetimes.txt");

            Assert.AreEqual(
                new DateTime(2012, 7, 19, 10, 23, 25, 544, DateTimeKind.Utc),
                payload.DateTimeWithOffset.ToUniversalTime());
        }

        [Test]
        public void Can_Deserialize_Nullable_DateTimeOffset_With_Value()
        {
            var payload = GetPayLoad<DateTimeTestStructure>("datetimes.txt");

            var expected = new DateTimeOffset(new DateTime(2011, 6, 30, 8, 15, 46, 929, DateTimeKind.Utc));

            Assert.IsNotNull(payload.NullableDateTimeOffsetWithValue);
            Assert.AreEqual(expected,payload.NullableDateTimeOffsetWithValue);
        }

        [Test]
        public void Can_Deserialize_Nullable_DateTimeOffset_With_Null()
        {
            var payload = GetPayLoad<DateTimeTestStructure>("datetimes.txt");

            Assert.IsNull(payload.NullableDateTimeOffsetWithNull);
        }

        [Test]
        public void Can_Deserialize_To_Dictionary_String_String()
        {
            var doc = CreateJsonStringDictionary();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var bd = d.Deserialize<Dictionary<string, string>>(response);

            Assert.AreEqual(bd["Thing1"], "Thing1");
            Assert.AreEqual(bd["Thing2"], "Thing2");
            Assert.AreEqual(bd["ThingRed"], "ThingRed");
            Assert.AreEqual(bd["ThingBlue"], "ThingBlue");
        }

        [Test]
        public void Can_Deserialize_To_Dictionary_String_String_With_Dynamic_Values()
        {
            var doc = CreateDynamicJsonStringDictionary();
            var d = new JsonDeserializer();
            var response = new RestResponse { Content = doc };
            var bd = d.Deserialize<Dictionary<string, string>>(response);

            Assert.AreEqual("[\"Value1\",\"Value2\"]", bd["Thing1"]);
            Assert.AreEqual("Thing2", bd["Thing2"]);
            Assert.AreEqual("{\"Name\":\"ThingRed\",\"Color\":\"Red\"}", bd["ThingRed"]);
            Assert.AreEqual("{\"Name\":\"ThingBlue\",\"Color\":\"Blue\"}", bd["ThingBlue"]);
        }

        [Test]
        public void Can_Deserialize_Decimal_With_Four_Zeros_After_Floating_Point()
        {
            const string json = "{\"Value\":0.00005557}";
            var response = new RestResponse() { Content = json };
            var d = new JsonDeserializer();
            var result = d.Deserialize<DecimalNumber>(response);

            Assert.AreEqual(result.Value, .00005557m);
        }

        [Test]
        public void Can_Deserialize_Object_Type_Property_With_Primitive_Vale()
        {
            var payload = GetPayLoad<ObjectProperties>("objectproperty.txt");

            Assert.AreEqual(42L, payload.ObjectProperty);
        }

        [Test]
        public void Can_Deserialize_Dictionary_of_Lists()
        {
            var doc = Unpack("jsondictionary.txt");

            var json = new JsonDeserializer();
            json.RootElement = "response";

            var output = json.Deserialize<EmployeeTracker>(new RestResponse { Content = doc });

            Assertions.NotEmpty(output.EmployeesMail);
            Assertions.NotEmpty(output.EmployeesTime);
            Assertions.NotEmpty(output.EmployeesPay);
        }

        private string CreateJsonWithUnderscores()
        {
            var doc = new JsonObject();
            doc["name"] = "John Sheehan";
            doc["start_date"] = new DateTime(2009, 9, 25, 0, 6, 1, DateTimeKind.Utc);
            doc["age"] = 28;
            doc["percent"] = 99.9999m;
            doc["big_number"] = long.MaxValue;
            doc["is_cool"] = false;
            doc["ignore"] = "dummy";
            doc["read_only"] = "dummy";
            doc["url"] = "http://example.com";
            doc["url_path"] = "/foo/bar";


            doc["best_friend"] = new JsonObject { 
                                    {"name", "The Fonz"},
                                    {"since", 1952}
                                };

            var friendsArray = new JsonArray();
            for (int i = 0; i < 10; i++)
            {
                friendsArray.Add(new JsonObject {
                                    {"name", "Friend" + i},
                                    {"since", DateTime.Now.Year - i}
                                });
            }

            doc["friends"] = friendsArray;

            var foesArray = new JsonObject{
                                {"dict1", new JsonObject{{"nickname", "Foe 1"}}},
                                {"dict2", new JsonObject{{"nickname", "Foe 2"}}}
                            };

            doc["foes"] = foesArray;

            return doc.ToString();
        }

        private string CreateJsonWithDashes()
        {
            var doc = new JsonObject();
            doc["name"] = "John Sheehan";
            doc["start-date"] = new DateTime(2009, 9, 25, 0, 6, 1, DateTimeKind.Utc);
            doc["age"] = 28;
            doc["percent"] = 99.9999m;
            doc["big-number"] = long.MaxValue;
            doc["is-cool"] = false;
            doc["ignore"] = "dummy";
            doc["read-only"] = "dummy";
            doc["url"] = "http://example.com";
            doc["url-path"] = "/foo/bar";

            doc["best-friend"] = new JsonObject{
                                    {"name", "The Fonz"},
                                    {"since", 1952}
                                };

            var friendsArray = new JsonArray();
            for (int i = 0; i < 10; i++)
            {
                friendsArray.Add(new JsonObject{
                                    {"name", "Friend" + i},
                                    {"since", DateTime.Now.Year - i}
                                });
            }

            doc["friends"] = friendsArray;

            var foesArray = new JsonObject{
                                {"dict1", new JsonObject{{"nickname", "Foe 1"}}},
                                {"dict2", new JsonObject{{"nickname", "Foe 2"}}}
                            };

            doc["foes"] = foesArray;

            return doc.ToString();
        }

        private string CreateIsoDateJson()
        {
            var bd = new Birthdate();
            bd.Value = new DateTime(1910, 9, 25, 9, 30, 25, DateTimeKind.Utc);

            return SimpleJson.SerializeObject(bd);
        }

        private string CreateUnixDateJson()
        {
            var doc = new JsonObject();
            doc["Value"] = 1309421746;

            return doc.ToString();
        }

        private string CreateJson()
        {
            var doc = new JsonObject();
            doc["Name"] = "John Sheehan";
            doc["StartDate"] = new DateTime(2009, 9, 25, 0, 6, 1, DateTimeKind.Utc);
            doc["Age"] = 28;
            doc["Percent"] = 99.9999m;
            doc["BigNumber"] = long.MaxValue;
            doc["IsCool"] = false;
            doc["Ignore"] = "dummy";
            doc["ReadOnly"] = "dummy";
            doc["Url"] = "http://example.com";
            doc["UrlPath"] = "/foo/bar";
            doc["Order"] = "third";
            doc["Disposition"] = "so_so";

            doc["Guid"] = new Guid(GuidString).ToString();
            doc["EmptyGuid"] = "";

            doc["BestFriend"] = new JsonObject{
                                    {"Name", "The Fonz"},
                                    {"Since", 1952}
                                };

            var friendsArray = new JsonArray();
            for (int i = 0; i < 10; i++)
            {
                friendsArray.Add(new JsonObject{
                                    {"Name", "Friend" + i},
                                    {"Since", DateTime.Now.Year - i}
                                });
            }

            doc["Friends"] = friendsArray;

            var foesArray = new JsonObject{
                                {"dict1", new JsonObject{{"Nickname", "Foe 1"}}},
                                {"dict2", new JsonObject{{"Nickname", "Foe 2"}}}
                            };

            doc["Foes"] = foesArray;

            return doc.ToString();
        }

        private string CreateJsonWithNullValues()
        {
            var doc = new JsonObject();
            doc["Id"] = null;
            doc["StartDate"] = null;
            doc["UniqueId"] = null;

            return doc.ToString();
        }

        private string CreateJsonWithEmptyValues()
        {
            var doc = new JsonObject();
            doc["Id"] = "";
            doc["StartDate"] = "";
            doc["UniqueId"] = "";

            return doc.ToString();
        }

        private string CreateJsonWithoutEmptyValues()
        {
            var doc = new JsonObject();
            doc["Id"] = 123;
            doc["StartDate"] = new DateTime(2010, 2, 21, 9, 35, 00, DateTimeKind.Utc);
            doc["UniqueId"] = new Guid(GuidString).ToString();

            return doc.ToString();
        }

        public string CreateJsonStringDictionary()
        {
            var doc = new JsonObject();
            doc["Thing1"] = "Thing1";
            doc["Thing2"] = "Thing2";
            doc["ThingRed"] = "ThingRed";
            doc["ThingBlue"] = "ThingBlue";
            return doc.ToString();
        }

        public string CreateDynamicJsonStringDictionary()
        {
            var doc = new JsonObject();
            doc["Thing1"] = new JsonArray { "Value1", "Value2" };
            doc["Thing2"] = "Thing2";
            doc["ThingRed"] = new JsonObject { { "Name", "ThingRed" }, { "Color", "Red" } };
            doc["ThingBlue"] = new JsonObject { { "Name", "ThingBlue" }, { "Color", "Blue" } };
            return doc.ToString();
        }

        private T GetPayLoad<T>(string fileName)
        {
            var doc = Unpack(fileName);
            var response = new RestResponse { Content = doc };
            var d = new JsonDeserializer();
            return d.Deserialize<T>(response);
        }

        private string Unpack(string resource)
        {
            string content = string.Empty;

            using (Stream s = asm.GetManifestResourceStream(BASE_NAME + resource))
            {
                
                StreamReader sr = new StreamReader(s);
                content = sr.ReadToEnd();
            }

            return content;
        }
    }
}
