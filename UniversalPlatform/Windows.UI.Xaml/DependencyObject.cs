using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace Windows.UI.Xaml
{
	public class DependencyObject
	{
		class DependencyPropertyContext
		{
			public DependencyProperty Property;
			public object Value;
		}

		readonly List<DependencyPropertyContext> _properties = new List<DependencyPropertyContext>(4);
		readonly Dictionary<long, Tuple<DependencyProperty, DependencyPropertyChangedCallback>> _callbacks = new Dictionary<long, Tuple<DependencyProperty, DependencyPropertyChangedCallback>>();
		long _callbackCount;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		DependencyPropertyContext GetContext(DependencyProperty dp)
		{
			List<DependencyPropertyContext> properties = _properties;

			for (var i = 0; i < properties.Count; i++) {
				var context = properties[i];
				if (ReferenceEquals(context.Property, dp))
					return context;
			}

			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		DependencyPropertyContext GetOrCreateContext(DependencyProperty dp)
		{
			return GetContext(dp) ?? CreateAndAddContext(dp);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		DependencyPropertyContext CreateAndAddContext(DependencyProperty dp)
		{
			var context = new DependencyPropertyContext {
				Property = dp,
				Value = DependencyProperty.UnsetValue,
			};

			_properties.Add(context);
			return context;
		}

		protected DependencyObject()
		{
		}

		public CoreDispatcher Dispatcher {
			get { throw new NotImplementedException(); }
		}

		public void ClearValue(DependencyProperty dp)
		{
			if (dp == null)
				throw new ArgumentNullException(nameof(dp));

			var context = GetContext(dp);
			if (context == null)
				return;

			var oldValue = context.Value;
			object newValue = DependencyProperty.UnsetValue;

			context.Value = newValue;

			OnPropertyChanged(dp, oldValue, newValue);
		}

		public object GetAnimationBaseValue(DependencyProperty dp)
		{
			throw new NotImplementedException();
		}

		public object GetValue(DependencyProperty dp)
		{
			if (dp == null)
				throw new ArgumentNullException(nameof(dp));

			var context = GetOrCreateContext(dp);
			if (ReferenceEquals(context.Value, DependencyProperty.UnsetValue))
				context.Value = dp.GetDefaultValue(GetType());

			return context.Value;
		}

		public object ReadLocalValue(DependencyProperty dp)
		{
			if (dp == null)
				throw new ArgumentNullException(nameof(dp));

			var context = GetContext(dp);
			return ReferenceEquals(null, context) ? DependencyProperty.UnsetValue: context.Value;
		}

		public long RegisterPropertyChangedCallback(DependencyProperty dp, DependencyPropertyChangedCallback callback)
		{
			var callbackToken = _callbackCount++;
			_callbacks.Add(callbackToken, new Tuple<DependencyProperty, DependencyPropertyChangedCallback>(dp, callback));
			return callbackToken;
		}

		public void SetValue(DependencyProperty dp, object value)
		{
			if (dp == null)
				throw new ArgumentNullException(nameof(dp));

			var context = GetOrCreateContext(dp);
			var oldValue = context.Value;
			context.Value = value;

			OnPropertyChanged(dp, oldValue, value);
		}

		public void UnregisterPropertyChangedCallback(DependencyProperty dp, long token)
		{
			if (dp == null)
				throw new ArgumentNullException(nameof(dp));

			_callbacks.Remove(token);
		}

		void OnPropertyChanged(DependencyProperty dp, object oldValue, object newValue)
		{
			if (Equals(oldValue, newValue))
				return;
			
			dp.OnPropertyChanged(this, oldValue, newValue);
			foreach (var cb in _callbacks.Values) {
				if (!ReferenceEquals(cb.Item1, dp))
					continue;
				cb.Item2(this, dp);
			}
		}
	}
}