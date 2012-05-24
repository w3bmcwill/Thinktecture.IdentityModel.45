using System;

namespace Thinktecture.IdentityModel
{
	public static class DateTimeEpochExtensions
	{
		/// <summary>
		/// Converts the given date value to epoch time.
		/// </summary>
		public static long ToEpochTime(this DateTime dateTime)
		{
			var date = dateTime.ToUniversalTime();
			var ts = date - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

			return Convert.ToInt64(ts.TotalSeconds);
		}

		/// <summary>
		/// Converts the given date value to epoch time.
		/// </summary>
		public static long ToEpochTime(this DateTimeOffset dateTime)
		{
			var date = dateTime.ToUniversalTime();
			var ts = date - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

			return Convert.ToInt64(ts.TotalSeconds);
		}

		/// <summary>
		/// Converts the given epoch time to a <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/> kind.
		/// </summary>
		public static DateTime ToDateTimeFromEpoch(this long secondsSince1970)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSince1970);
		}

		/// <summary>
		/// Converts the given epoch time to a UTC <see cref="DateTimeOffset"/>.
		/// </summary>
		public static DateTimeOffset ToDateTimeOffsetFromEpoch(this long secondsSince1970)
		{
			return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(secondsSince1970);
		}
	}
}