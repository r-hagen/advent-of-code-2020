using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles
{
    public class PassportProcessing
    {
        public int Solve1(string inputFile)
        {
            var valid = 0;
            var passports = ParseInput(inputFile);

            foreach (var passport in passports)
                if (hasRequiredFields(passport)) valid++;

            return valid;
        }

        public int Solve2(string inputFile)
        {
            var valid = 0;
            var passports = ParseInput(inputFile);

            foreach (var passport in passports)
                if (hasRequiredFields(passport) && hasValidFields(passport))
                    valid++;

            return valid;
        }

        public string[] PassportFields()
        {
            return typeof(PassportField)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.Name)
                .ToArray();
        }

        private bool hasRequiredFields(Dictionary<string, string> passport)
        {
            if (PassportFields().All(f => passport.Keys.Contains(f)))
                return true;

            var missingFields = PassportFields().Where(f => !passport.Keys.Contains(f));
            if (missingFields.Count() == 1 && missingFields.First() == PassportField.cid)
                return true;

            return false;
        }

        private bool hasValidFields(Dictionary<string, string> passport)
        {
            if (!IsValidYear(passport[PassportField.byr], 1920, 2002))
                return false;

            if (!IsValidYear(passport[PassportField.iyr], 2010, 2020))
                return false;

            if (!IsValidYear(passport[PassportField.eyr], 2020, 2030))
                return false;

            if (!isValidHeight(passport[PassportField.hgt]))
                return false;

            if (!isValidHairColor(passport[PassportField.hcl]))
                return false;

            if (!isValidEyeColor(passport[PassportField.ecl]))
                return false;

            if (!isValidPassportId(passport[PassportField.pid]))
                return false;

            return true;
        }

        public bool IsValidYear(string year, int min, int max)
        {
            if (year.Length != 4)
                return false;

            if (!int.TryParse(year, out var byr) || (byr < min || byr > max))
                return false;

            return true;
        }

        private bool isValidHeight(string height)
        {
            var validUnits = new[] { "cm", "in" };
            var regex = new Regex("^(?<number>[0-9]+)(?<unit>(cm|in){1})$");

            var match = regex.Match(height);
            if (!match.Success)
                return false;

            var unit = match.Groups["unit"].Value;
            if (!validUnits.Contains(unit))
                return false;

            if (!int.TryParse(match.Groups["number"].Value, out var number))
                return false;

            if (unit == "cm" && (number < 150 || number > 193))
                return false;
            else if (unit == "in" && (number < 59 || number > 76))
                return false;

            return true;
        }

        private bool isValidHairColor(string color)
        {
            var regex = new Regex("^#([0-9]|[a-f]){6}$");
            return regex.Match(color).Success;
        }

        private bool isValidEyeColor(string color)
        {
            var validColors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            if (!validColors.Contains(color))
                return false;

            return true;
        }

        private bool isValidPassportId(string id)
        {
            if (id.Length != 9)
                return false;

            if (!id.All(c => char.IsDigit(c)))
                return false;

            return true;
        }

        public List<Dictionary<string, string>> ParseInput(string inputFile)
        {
            var result = new List<Dictionary<string, string>>();

            Func<Dictionary<string, string>> preparePassport = () =>
            {
                var passport = new Dictionary<string, string>();
                result.Add(passport);
                return passport;
            };

            var passport = preparePassport();
            var lines = File.ReadAllLines(inputFile);
            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    passport = preparePassport();
                    continue;
                }

                var fieldPairs = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var fp in fieldPairs)
                {
                    var kvp = fp.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    passport.Add(kvp[0], kvp[1]);
                }
            }

            return result;
        }
    }

    public class PassportField
    {
        public const string byr = nameof(byr);
        public const string iyr = nameof(iyr);
        public const string eyr = nameof(eyr);
        public const string hgt = nameof(hgt);
        public const string hcl = nameof(hcl);
        public const string ecl = nameof(ecl);
        public const string pid = nameof(pid);
        public const string cid = nameof(cid);
    }
}
