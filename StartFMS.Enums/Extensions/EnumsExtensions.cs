using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFMS.Enums.Extensions
{
    static class EnumsExtensions
    {
        /// <summary>
        /// 取得列舉的 DisplayAttribute.Name
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string? GetDisplayName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.Name;
        }

        /// <summary>
        /// 取得列舉的 DisplayAttribute.Description
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string? GetDescription(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.Description;
        }

        /// <summary>
        /// 取得列舉的 DisplayAttribute.GroupName
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string? GetGroupName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.GroupName;
        }

        /// <summary>
        /// 取得列舉的 DisplayAttribute.Order
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static int GetOrder(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.Order ?? 0;
        }

        /// <summary>
        /// 取得列舉的 DisplayAttribute.ShortName
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string? GetShortName(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.ShortName;
        }

        /// <summary>
        /// 取得列舉的 DisplayAttribute.Prompt
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string? GetPrompt(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.Prompt;
        }

        /// <summary>
        /// 取得列舉的 DisplayAttribute.AutoGenerateField
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static bool GetAutoGenerateField(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.AutoGenerateField ?? false;
        }

        /// <summary>
        /// 取得列舉的 DisplayAttribute.AutoGenerateFilter
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static bool GetAutoGenerateFilter(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false).FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
            return displayAttribute?.AutoGenerateFilter ?? false;
        }

        /// <summary>
        /// 取得列舉清單
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static IEnumerable<Enum> GetEnumList(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            return Enum.GetValues(enumType).Cast<Enum>();
        }

        /// <summary>
        /// 取得列舉清單的 DisplayAttribute.Name
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetEnumListDisplayNames(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var enumValues = Enum.GetValues(enumType).Cast<Enum>();

            if (enumValues == null)
            {
                return new List<string>();
            }

            // 如果 GetDisplayName() 回傳 null，則 throw exception
            return enumValues!.Select(e => e.GetDisplayName() ?? throw new Exception("DisplayAttribute.Name is null"));
        }

        
    }
}
