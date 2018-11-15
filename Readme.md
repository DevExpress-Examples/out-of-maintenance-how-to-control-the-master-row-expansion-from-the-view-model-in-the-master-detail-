<!-- default file list -->
*Files to look at*:

* [DataClass.cs](./CS/WPFDataGridApp15/DataClass.cs) (VB: [DataClass.vb](./VB/WPFDataGridApp15/DataClass.vb))
* [MainWindow.xaml](./CS/WPFDataGridApp15/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/WPFDataGridApp15/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/WPFDataGridApp15/MainWindow.xaml.cs) (VB: [MainWindow.xaml](./VB/WPFDataGridApp15/MainWindow.xaml))
* [MasterDetailMVVMHelper.cs](./CS/WPFDataGridApp15/MVVMHelper/MasterDetailMVVMHelper.cs) (VB: [MasterDetailMVVMHelper.vb](./VB/WPFDataGridApp15/MVVMHelper/MasterDetailMVVMHelper.vb))
* [ViewModel.cs](./CS/WPFDataGridApp15/ViewModel.cs) (VB: [ViewModel.vb](./VB/WPFDataGridApp15/ViewModel.vb))
<!-- default file list end -->
# How to control the master row expansion from the view model in the Master-Detail grid


<p>This example illustrates how to add MVVM capabilities to the Master-Detail functionality. In this example we have created the MasterDetailMVVMHelper class that introduces the ExpandedMasterRowsSource attached property. This property is intended to be attached to GridControl and allows controlling details' expanded state via a collection in a view model.</p>
<p>Here is a sample XAML that illustrates how to attach this property to GridControl and make it bound to the ExpandedItems collection:</p>


```xaml
<dxg:GridControl my:MasterDetailMVVMHelper.ExpandedMasterRowsSource="{Binding ExpandedItems}" Name="gridControl1" ItemsSource="{Binding Data}" >
```


<p> <br> Now, by adding a couple of data objects to the ExpandedItems collection, you can expand corresponding rows in GridControl:</p>


```cs
ExpandedItems.Add(Data[1]);
ExpandedItems.Add(Data[10]);

```




```vb
ExpandedItems.Add(Data(1))
ExpandedItems.Add(Data(10))

```


<p><strong><u>An </u></strong><strong><u>important note</u></strong><strong>:</strong> The collection being bound to the ExpandedMasterRowsSource must be an implementation of the <strong>I</strong><strong>NotifyC</strong><strong>ollectionChanged</strong> interface, otherwise this functionality will not operate as expected.</p>

<br/>


