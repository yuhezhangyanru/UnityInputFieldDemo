# UnityInputFieldDemo
this is a demo to guide you to use a inputfiled component on device phone

这个例子是为了说明怎么在设备上使用InputField或设备输入法的一些参数。
假如在设备上，InputField组件显示在屏幕中，键盘输入结束，InputField.text就是手输入的文本，但是如果InputField组件在屏幕外或者被隐藏，取值将都拿不到键盘输入的值，因此，保险的做法干脆是通过 TouchScreenKeyboard 输入法对象参数或静态参数去获取输入法相关的状态！