using Microsoft.AspNetCore.Components;
using System;

namespace BlazorWebGUI.Helpers
{
    public static class DynamicRender
    {

		/// <summary>
		/// Loads any Razor Page / Component dynamically into current Razor component (as child)
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static RenderFragment RenderRazorComponent(Type type)
		{
			RenderFragment renderFragment = renderTreeBuilder =>
			{
				renderTreeBuilder.OpenComponent(0, type);
				renderTreeBuilder.CloseComponent();
			};
			return renderFragment;
		}

	}
}
