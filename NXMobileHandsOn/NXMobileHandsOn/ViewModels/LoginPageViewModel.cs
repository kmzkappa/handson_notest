using NXMobileHandsOn.ServiceReference;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace NXMobileHandsOn.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {

        // 会社コード
        public ReactiveProperty<string> KaiCode { get; set; }
            = new ReactiveProperty<string>();

        // ユーザーID
        public ReactiveProperty<string> UserId { get; set; }
            = new ReactiveProperty<string>();

        // パスワード
        public ReactiveProperty<string> Password { get; set; }
            = new ReactiveProperty<string>();

        // ログイン日付
        public ReactiveProperty<DateTime> LoginDate { get; set; }
            = new ReactiveProperty<DateTime>();

        // エラーメッセージ
        public ReactiveProperty<string> ErrorMessage { get; set; }
            = new ReactiveProperty<string>();

        // ログインボタンの処理
        public ReactiveCommand LoginCommand { get; set; }
            = new ReactiveCommand();

        // NavigationService
        private INavigationService _navigationService;

        public LoginPageViewModel(INavigationService navigationService)
        {
            KaiCode.Value = "NXSYS";
            LoginCommand.Subscribe(LoginButtonTapped);
            _navigationService = navigationService;
        }

        // ログインボタンの押下時処理
        private async void LoginButtonTapped(object sender)
        {
            var loginService = new LoginService();
            NLC00100SIParamV2 result = null;
            try
            {
                // ログインサービスの呼び出し
                result = await loginService.LoginAsync(KaiCode.Value, UserId.Value, Password.Value, LoginDate.Value);
            }
            catch (Exception ex)
            {
                ErrorMessage.Value = ex.Message;
            }

            // ログインに成功すると認証キー'NshKey'が設定される
            if (result?.NshKey != null)
            {
                ErrorMessage.Value = string.Empty;
                await _navigationService.NavigateAsync("MenuPage");
            }
            else
            {
                ErrorMessage.Value = result.clientMessageList.FirstOrDefault().StatusMessage;
            }
        }
    }
}
