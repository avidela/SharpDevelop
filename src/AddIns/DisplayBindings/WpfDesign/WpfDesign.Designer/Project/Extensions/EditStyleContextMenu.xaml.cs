﻿// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using ICSharpCode.WpfDesign.Designer.PropertyGrid.Editors.FormatedTextEditor;
using ICSharpCode.WpfDesign.Designer.Xaml;
using ICSharpCode.WpfDesign.XamlDom;

namespace ICSharpCode.WpfDesign.Designer.Extensions
{
	public partial class EditStyleContextMenu
	{
		private DesignItem designItem;

		public EditStyleContextMenu(DesignItem designItem)
		{
			this.designItem = designItem;
			
			InitializeComponent();
		}

		void Click_EditStyle(object sender, RoutedEventArgs e)
		{
			var element = designItem.View;
			object defaultStyleKey = element.GetValue(FrameworkElement.DefaultStyleKeyProperty);
			Style style = Application.Current.TryFindResource(defaultStyleKey) as Style;

			var service = ((XamlComponentService) designItem.Services.Component);

			var ms = new MemoryStream();
			XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			XamlWriter.Save(style, writer);

			var rootItem = this.designItem.Context.RootItem as XamlDesignItem;

			ms.Position = 0;
			var sr = new StreamReader(ms);
			var xaml = sr.ReadToEnd();

			var xamlObject = XamlParser.ParseSnippet(rootItem.XamlObject, xaml, ((XamlDesignContext)this.designItem.Context).ParserSettings);
			
			var styleDesignItem=service.RegisterXamlComponentRecursive(xamlObject);
			designItem.Properties.GetProperty("Resources").CollectionElements.Add(styleDesignItem);
		}
	}
}