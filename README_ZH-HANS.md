<div align=center>

# OpenGptChat 

[![EN-US](https://img.shields.io/badge/EN-US-blue)](README.md) [![ZH-HANS](https://img.shields.io/badge/中文-简体-red)](README_ZH-HANS.md) [![ZH-HANT](https://img.shields.io/badge/中文-繁体-red)](README_ZH-HANT.md) [![ZH-HANT](https://img.shields.io/badge/TR-TR-red)](README_TR.md) / [![release date](https://img.shields.io/github/release-date/SlimeNull/OpenGptChat)](https://github.com/SlimeNull/OpenGptChat/releases) [![stars](https://img.shields.io/github/stars/SlimeNull/OpenGptChat?style=flat)](https://github.com/SlimeNull/OpenGptChat/pulse)

基于 [Open AI Chat API](https://platform.openai.com/docs/guides/chat) 的简易聊天客户端

![预览](assets/preview3.png)

</div>

## 功能

1. 实时响应. 程序使用 `SSE` (`Server-Sent Events`) 推送接受数据, 服务器响应时程序会立即输出到屏幕.
2. 多会话. 可以在程序左侧创建会话, 会话有独立的上下文, 可以随意切换.
3. 多语言. 第一次启动时, 程序会检测你的系统语言, 自动切换到支持的语言, 也可以在配置中手动切换.
4. 热更新. 配置修改后立即生效, 包括置顶选项与语言设置.
5. 热键. 程序提供了隐藏 (`Ctrl+H`) 与还原 (`Ctrl+Shift+H`) 热键, 可以在任何处唤起.

## 使用方法

1. 在 `Releases` 中下载最新版.
2. 新建文件夹, 并将 `OpenGptChat.exe` 移动到文件夹内.
3. 打开 `OpenGptChat.exe`, 程序将自动在当前位置生成配置文件.
4. 到配置页面设置 `API 密钥`
5. 坐和放宽, 享受 `OpenGptChat` 带来的乐趣罢!

> 小提示：您可以在文本框中使用 `Ctrl + Enter` 发送消息。

## 什么是 API 密钥

OpenAI 的 API 使用 API 密钥登录，并根据 API 密钥扣费。前往[API 密钥](https://platform.openai.com/account/api-keys)新建并复制你的 API 密钥。

## 您可以学到什么？

1. 学习 WPF 中的 **Binding**、**Command**、**Template**、**Style**、**Trigger**、**Animation**
2. 使用 `LiteDB` 保存数据而无需编写 SQL 语句.
3. 使用 `CommunityToolkit.Mvvm` 以便捷的方式实现可绑定数据与命令.
4. 使用 `Microsoft.Extensions.Hosting` 进行服务管理, 配置与依赖注入
5. 使用 `Hardcodet.NotifyIcon.Wpf` 在 WPF 程序中创建通知图标.
6. 使用 `Microsoft.Xaml.Behaviors.Wpf` 在 WPF 程序中添加更多操作方式.
7. 使用 `EleCho.GlobalHotkey.Windows.Wpf` 在 WPF 程序中处理全局热键.

## FAQ

- Q: 为什么我设置的 `系统消息` 没有生效? \
  A: 由于 `系统消息` 的特殊性, 你需要在设置 `系统消息` 后点击 `应用` 按钮.
