<div align="center">

![logo](./assets/icon.svg)

# WandEnhancer

[![GitLab Mirror](https://img.shields.io/badge/GitLab-mirror-fc6d26?logo=gitlab)](https://gitlab.com/kitbyte/wand-enhancer)

</div>

<h4>An open-source interoperability tool designed to extend local client-side configurations and improve the UX of the Wand application.</h4>

<p>🌐 <b>English</b> · <a href="#tieng-viet">Tiếng Việt</a></p>

**🚨 IMPORTANT NOTICE: THIS PROJECT HAS NO OFFICIAL YOUTUBE TUTORIALS, GUIDES, OR PREBUILT EXECUTABLE DOWNLOADS. 🚨
There are no official videos showing how to install or use this tool. Scammers are creating fake tutorials using this project's name and placing malware/password stealers in the video descriptions. Official GitHub releases contain release notes only, not `.exe` files. If you downloaded an `.exe` or archive from a YouTube link, a random website, or a third-party mirror, you did not get it from this project. We are not responsible for third-party downloads.**

## 👾 What does it access?

The default .NET patcher modifies files in the selected local Wand installation and contains no update-checking or telemetry network code. Wand itself remains an online application, build tools restore declared dependencies, and the optional Remote Web Panel deliberately starts a LAN HTTP/WebSocket server and uses Wand API/CDN data. An explicit build-time option can include GitHub release notifications; that variant sends a GitHub API request with your IP and a User-Agent when Wand starts, but sends no Wand or account data and never downloads updates. Review the source and build the executable from your own fork; unsigned patching tools can trigger generic antivirus heuristics.

## 💫 What features are improved?

✅ Local environment configuration management <br/>
✅ Automated compatibility adjustments for new client versions <br/>
✅ Advanced layout and theme customization (Client-side only) <br/>
✅ AI Features <br/>
✅ Remote web panel (Remote Connect on mobile) <br/>
✅ Vietnamese localization — patch adds *Tiếng Việt* to the client's Language list (Settings → General) <br/>

## 🌐 Remote Web Panel
WandEnhancer includes a built-in **Remote Web Panel** allowing you to control app features directly from your phone.

### Quick Start:
1. Ensure both your PC and phone are on the **same Wi-Fi network**.
2. Hover over the **Connect** button in the top bar of WandEnhancer.
3. Scan the displayed **QR code** with your phone's camera.

### Troubleshooting & Remote Access:
- **Page isn't loading?** First, ensure both your PC and phone are connected to the **same local network**. Some routers and guest Wi-Fi networks enable client isolation/AP isolation, which blocks devices on the same SSID from reaching each other. If it still does not load, check Windows Firewall and allow inbound traffic on TCP port `3223` for your local network. If Windows marked your connection as **Public**, switching it to **Private** can also help.
- **Using mobile data or a different network?** If you want to use the panel over mobile data (LTE/5G) or from an entirely different network, you can use [Tailscale](https://tailscale.com/) or similar VPN tools.
- The panel uses plain HTTP on port `3223` and has no pairing code. Anyone who can reach that port can view the panel and control the active trainer, so use it only on a trusted LAN/VPN and never expose the port directly to the internet.
- The panel protocol does not include your Wand bearer token or installation-path fields.

## 👀 How to use?

This repository does not publish official compiled binaries. Build your own executable from your own fork using GitHub Actions.

1. Sign in to GitHub and fork this repository.
2. Use **Sync fork** before each build so your fork contains the latest fixes.
3. Open your fork, go to the **Actions** tab, and enable workflows if GitHub asks you to.
4. Select the **Build executable** workflow.
5. Click **Run workflow** and start the run. Leave **Include GitHub release checks when Wand starts** off for a fully offline patcher, or enable it to compile in new-version notifications.
6. Wait for the workflow to finish, open the completed run, and download the artifact.
7. Extract the artifact zip and run `WandEnhancer.exe` to apply local client modifications.

### Testing a release candidate

- `master` is the stable source. Select a `feature/rc_*` branch in your fork's **Run workflow** branch selector only when the maintainer explicitly asks for candidate testing. Ensure that branch contains the upstream commit you intend to test; syncing `master` does not update a separate RC branch.
- You do **not** need to open a pull request to this repository to build your fork.
- Record the workflow's source commit SHA, not just `2.0.0.0`: the RC tag, RC branch and a local build may contain different fixes.
- For startup failures, attach `launcher.log` and, if relevant, `launcher.prev.log` from the Wand installation root. They include the build commit and applied patches. Remove personal paths or other private information before sharing.
- Include the exact Wand version and stable/beta channel, selected patches, and whether the failure happened on a fresh install, an update, or Restore. Do not attach executables, account tokens or storage dumps.

*Here how you do it:*

https://github.com/user-attachments/assets/7966cabe-0aa6-424d-8c2f-981ad91e0f91



## 🇻🇳 Vietnamese localization patch

**Download:** grab `WandEnhancer.exe` directly from release [2.2.0.0](../../releases/tag/2.2.0.0) (or this fork's [Releases](../../releases) page) — it ships the prebuilt patcher with the Vietnamese bundle baked in, plus a step-by-step Vietnamese install guide in the release notes.

Tick **Tiếng Việt** in the patch dialog (row between *Remote Web Panel preview* and *Auto-apply after updates*) before patching:

```
[Patch ON] VietnameseLocale (32)
   ├─► supported-locales  += "vi-VN"          (i18n bundle)
   ├─► language map       += vi:"Tiếng Việt"  (native name in dropdown)
   └─► static/strings/vi-VN.json packed       (2,385 translated strings)
                    ▼
Settings › General › Language  →  select "Tiếng Việt"
                    ▼
Whole client UI switches to Vietnamese instantly (no restart).
```

The patch is language-neutral: it only *adds* Vietnamese to the list — the app keeps following *Automatic (English)* on an en-US Windows until you pick *Tiếng Việt*. Verified live on Wand 12.58.0: both anchors survive minified and prettified bundles (`node --check` clean) and the settings page renders full Vietnamese with correct diacritics.

## 🧩 Custom scripts

You can inject your own JavaScript into Wand at patch time to tweak or fix things in the client UI. This reuses the same renderer injection the Remote Web Panel uses, so it requires the **Remote Web Panel** patch to be enabled.

**How to add a script**

- In the patch dialog, add one or more `.js` files (only existing `.js` files are accepted), **or**
- Drop `.js` files into a `renderer-scripts/` folder placed next to the patcher executable.

Then patch as usual — your scripts are bundled into the client and run inside Wand's window.

**How it runs**

- Each script runs inside Wand's renderer (full DOM access, plus Node `require`).
- It is wrapped so a thrown error is logged and never crashes Wand.
- It may run **more than once** per launch (on load and again shortly after), so guard one‑time work behind a global flag.
- A small `WandEnhancer` helper is available: `WandEnhancer.log(...)`, `WandEnhancer.remoteUrl`, `WandEnhancer.apiVersion`.

**Minimal example** (`hello.js`)

```js
// Injected scripts can run multiple times — guard one-time setup.
if (!globalThis.__helloScriptInstalled) {
  globalThis.__helloScriptInstalled = true;

  WandEnhancer.log("Hello from my custom script!", WandEnhancer.remoteUrl);

  new MutationObserver(() => {
    const dialog = document.querySelector("ux-dialog:not([data-seen])");
    if (dialog) {
      dialog.setAttribute("data-seen", "1");
      WandEnhancer.log("A dialog opened.");
    }
  }).observe(document.documentElement, { childList: true, subtree: true });
}
```

> Scripts run with the same privileges as the Wand client. Only add scripts you trust and understand.

## 🛠️ How to build from source

Building from source on Windows requires a local development environment.

### Requirements

- `Node.js` and `pnpm`
- `Visual Studio 2022` or `Build Tools for Visual Studio 2022` with `MSBuild`
- .NET Framework 4.8 desktop build tools / targeting pack

### Build steps

1. Clone this repository.
2. Install the requirements above and make sure `pnpm` and `MSBuild` are available.
3. Run `build.cmd` from Command Prompt or PowerShell.

The build script installs dependencies, lints and type-checks the panel, builds production assets, runs web tests, builds WPF, and checks desktop patch state and structural JavaScript patches. Tests use temporary fixtures, not your Wand installation.

Update notifications are excluded by default. To compile them in locally, run `build.cmd -EnableUpdateNotifications`. When compiled in, the check runs on Wand's launch (on by default, toggle in Settings), shows a native Windows notification for a newer release, and opens the release notes when clicked (the release page when only the launcher is running). It never downloads or installs an update.

---

## ❓ Q&A

- **Why is there no `.exe` in GitHub Releases?**
  - Official releases are notes-only on purpose. The project no longer distributes prebuilt executables because unsigned or self-built patching tools are repeatedly reuploaded, mislabeled, and flagged by third-party scanners. Build the executable from your own fork using GitHub Actions instead.
- **Where do I download the executable?**
  - From your own fork's **Actions** artifact after running the **Build executable** workflow, or from a **Releases** page of your own fork when you publish one (this fork's [2.2.0.0](../../releases/tag/2.2.0.0) release ships the prebuilt Vietnamese-enabled patcher). Do not download `.exe` files from YouTube descriptions, random mirrors, Discord attachments, or issue comments.
- **Why does Windows Defender or SmartScreen warn about my build?**
  - The GitHub Actions artifact is unsigned and uncommon, so Windows may warn even when the code was built directly from your fork. Review the source, verify the workflow logs, and only run binaries you built yourself.
- **Can I use a binary built by someone else?**
  - You can, but you should treat it as untrusted. This repository cannot verify or support third-party builds.
- **Does this send data anywhere?**
  - The default .NET patcher is fully offline. The optional Remote Web Panel listens on your LAN and may request trainer translations/artwork through Wand's existing API/CDN paths. If you explicitly compile in update notifications, each Wand launch checks GitHub's public releases API and exposes only the normal request metadata, including your IP and User-Agent. There is no telemetry, download, or automatic update.
- **How do I learn about a new version without an in-app update check?**
  - On GitHub choose **Watch → Custom → Releases**, then sync your fork and run **Build executable** when a release is published. You can also opt into compile-time release notifications in the manual workflow.

---
## 🖼️ Screenshots
![1](./assets/screenshots/app1.png)
<div align='center'>

![2](./assets/screenshots/app2.png)
</div>


<a id="tieng-viet"></a>

## 🇻🇳 Tiếng Việt

*Bản dịch tiếng Việt của README — bản gốc tiếng Anh nằm ở phía trên trang.*

### ⚠️ Cảnh báo quan trọng

**🚨 DỰ ÁN NÀY KHÔNG CÓ VIDEO HƯỚNG DẪN, BÀI HƯỚNG DẪN HAY FILE .exe CHÍNH THỨC NÀO TRÊN YOUTUBE. 🚨**
Không có video chính thức nào hướng dẫn cài đặt hay sử dụng công cụ này. Kẻ lừa đảo đang dùng tên dự án để làm video giả và đặt mã độc/steal mật khẩu trong phần mô tả. Bản release chính thức trên GitHub chỉ có ghi chú phát hành, không kèm file `.exe`. Nếu bạn tải `.exe` hoặc file nén từ link YouTube, trang web lạ hay trang tải bên thứ ba thì **không phải từ dự án này**. Chúng tôi không chịu trách nhiệm với file tải từ bên thứ ba.

### 👾 Công cụ này truy cập gì?

Patcher .NET mặc định chỉ sửa file trong thư mục cài Wand cục bộ, **không** chứa mã kiểm tra cập nhật hay telemetri. Wand vẫn là ứng dụng online; công cụ build tải đúng các dependency được khai báo; Remote Web Panel (tùy chọn) chủ động mở server HTTP/WebSocket trên mạng LAN và dùng dữ liệu từ API/CDN của Wand. Tùy chọn build có thể bật thông báo release qua GitHub (chỉ gửi một request tới GitHub kèm IP + User-Agent khi Wand khởi động, không gửi dữ liệu Wand/tài khoản, không bao giờ tải hay cài cập nhật). Hãy xem mã nguồn và tự build exe từ fork của bạn; công cụ patch chưa chữ ký có thể bị antivirus cảnh báo false positive.

### 💫 Tính năng được cải thiện

✅ Quản lý cấu hình môi trường cục bộ <br/>
✅ Tự động điều chỉnh tương thích cho phiên bản client mới <br/>
✅ Tùy chỉnh giao diện & chủ đề nâng cao (chỉ phía client) <br/>
✅ Tính năng AI <br/>
✅ Remote Web Panel (điều khiển Wand từ điện thoại) <br/>
✅ Hỗ trợ tiếng Việt — patch thêm *Tiếng Việt* vào danh sách Ngôn ngữ (Cài đặt → Chung) <br/>

### 🌐 Remote Web Panel (điều khiển từ điện thoại)

WandEnhancer có sẵn **Remote Web Panel** để bạn điều khiển các tính năng ngay từ điện thoại.

**Bắt đầu nhanh:**
1. Đảm bảo PC và điện thoại cùng mạng **Wi-Fi**.
2. Di chuột lên nút **Connect** trên thanh công cụ của WandEnhancer.
3. Quét **mã QR** hiển thị bằng camera điện thoại.

**Xử lý sự cố & truy cập từ xa:**
- **Trang không tải được?** Kiểm tra cả hai thiết bị cùng **mạng nội bộ**. Một số router/wifi khách bật cô lập client (AP isolation) khiến các thiết bị cùng SSID không thấy nhau. Nếu vẫn không được, mở Windows Firewall cho phép inbound TCP cổng `3223`; nếu kết nối đang ở chế độ **Public**, đổi sang **Private** cũng có ích.
- **Dùng dữ liệu di động hoặc mạng khác?** Muốn dùng panel qua LTE/5G hay mạng hoàn toàn khác, hãy dùng [Tailscale](https://tailscale.com/) hoặc công cụ VPN tương tự.
- Panel chạy HTTP thường trên cổng `3223`, không có mã ghép cặp. Ai chạm được tới cổng này đều xem được panel và điều khiển trainer đang chạy — chỉ dùng trên LAN/VPN đáng tin cậy, **không** expose cổng ra internet.
- Giao thức panel **không** chứa bearer token Wand hay đường dẫn cài đặt của bạn.

### 👀 Cách dùng

Kho lưu trữ này **không** đăng file exe chính thức. Hãy tự build exe từ fork của bạn qua GitHub Actions:

1. Đăng nhập GitHub và fork kho này.
2. Dùng **Sync fork** trước mỗi lần build để fork có bản sửa lỗi mới nhất.
3. Mở fork → tab **Actions** → bật workflows nếu GitHub yêu cầu.
4. Chọn workflow **Build executable**.
5. Bấm **Run workflow**. Giữ tắt **Include GitHub release checks when Wand starts** để patcher hoàn toàn offline, hoặc bật nếu muốn compile thêm thông báo bản mới.
6. Chờ workflow chạy xong, mở run đã hoàn tất rồi tải artifact.
7. Giải nén artifact và chạy `WandEnhancer.exe` để áp dụng các bản vá client cục bộ.

*Xem video minh họa ở mục tiếng Anh phía trên.*

### 🇻🇳 Patch Việt hóa — cài đặt chi tiết

**Tải về:** lấy `WandEnhancer.exe` trực tiếp từ release [2.2.0.0](../../releases/tag/2.2.0.0) (hoặc trang [Releases](../../releases) của fork này) — bản này đã kèm sẵn patcher với bundle tiếng Việt, kèm hướng dẫn cài từng bước trong ghi chú phát hành.

**Các bước cài:**
1. Tải `WandEnhancer.exe` từ mục **Assets** của release.
2. Đóng Wand hoàn toàn (Quit từ khay + kiểm tra Task Manager cho chắc).
3. Chạy `WandEnhancer.exe` — nếu SmartScreen chặn: **More info → Run anyway** (exe chưa chữ ký nên Windows cảnh báo là bình thường).
4. Trong hộp thoại patch, tick **Tiếng Việt** (dòng nằm giữa *Remote Web Panel preview* và *Auto-apply after updates*); các option khác (Activate Pro...) tùy chọn.
5. Bấm **Patch**, chờ khoảng 1 phút (chương trình tự backup 2 file trước khi sửa).
6. Mở Wand → avatar góc phải → **Cài đặt** → **Chung** → **Ngôn ngữ** → chọn **Tiếng Việt** → toàn bộ giao diện chuyển ngay, không cần mở lại.
7. Muốn gỡ: mở patcher → bấm **Restore**.

```
[Patch ON] VietnameseLocale (32)
   ├─► supported-locales  += "vi-VN"          (bundle i18n)
   ├─► language map       += vi:"Tiếng Việt"  (tên bản xứ trong dropdown)
   └─► static/strings/vi-VN.json packed       (2.385 chuỗi đã dịch)
                    ▼
Cài đặt › Chung › Ngôn ngữ  →  chọn "Tiếng Việt"
                    ▼
Toàn bộ giao diện client chuyển tiếng Việt ngay lập tức (không restart).
```

Patch trung lập ngôn ngữ: nó chỉ *thêm* tiếng Việt vào danh sách — Windows en-US vẫn theo *Automatic (English)* cho tới khi bạn tự chọn *Tiếng Việt*. Đã test trực tiếp trên Wand 12.58.0: cả hai neo chịu được bundle nén lẫn tinh giản (`node --check` sạch) và trang cài đặt hiển thị đủ dấu tiếng Việt.

### 🧩 Script tùy chỉnh

Bạn có thể tiêm JavaScript riêng vào Wand lúc patch để tinh chỉnh/gỡ lỗi giao diện client. Đây là cùng cơ chế renderer injection với Remote Web Panel nên cần bật patch **Remote Web Panel**.

**Cách thêm script**
- Trong hộp thoại patch, thêm một hoặc nhiều file `.js` (chỉ nhận file `.js` đã tồn tại), **hoặc**
- Đặt file `.js` vào thư mục `renderer-scripts/` cạnh file patcher.

Rồi patch như bình thường — script được đóng gói vào client và chạy trong cửa sổ Wand.

**Cách chạy**
- Mỗi script chạy trong renderer của Wand (toàn quyền DOM + `require` của Node).
- Script được bọc để lỗi chỉ được ghi log, không bao giờ làm Wand crash.
- Có thể chạy **nhiều hơn một lần** mỗi lần mở app (lúc load và ngay sau đó) — việc chạy một lần nên đặt sau cờ toàn cục.
- Có helper `WandEnhancer`: `WandEnhancer.log(...)`, `WandEnhancer.remoteUrl`, `WandEnhancer.apiVersion`.

> Script chạy với đúng đặc quyền của client Wand. Chỉ thêm script bạn tin tưởng và hiểu rõ.

### 🛠️ Build từ nguồn

Build trên Windows cần môi trường phát triển cục bộ.

**Yêu cầu**
- `Node.js` và `pnpm`
- `Visual Studio 2022` hoặc `Build Tools for Visual Studio 2022` kèm `MSBuild`
- Bộ công cụ/build pack .NET Framework 4.8 desktop

**Các bước build**
1. Clone kho này.
2. Cài yêu cầu trên, đảm bảo `pnpm` và `MSBuild` chạy được.
3. Chạy `build.cmd` từ Command Prompt hoặc PowerShell.

Script build sẽ cài dependency, lint và type-check panel, build bản production, chạy test web, build WPF, rồi kiểm tra trạng thái patch và các patch JavaScript cấu trúc. Test dùng fixture tạm, **không** đụng tới cài Wand của bạn.

Thông báo cập nhật bị loại trừ theo mặc định; muốn bật local thì chạy `build.cmd -EnableUpdateNotifications`.

### ❓ Hỏi–Đáp

- **Tại sao release chính thức không có `.exe`?**
  - Bản release chính thức cố tình chỉ có ghi chú — dự án không phân phối exe dựng sẵn vì công cụ patch chưa chữ ký/tự build thường bị người khác tải lại, gắn nhãn sai và bị scanner cảnh báo. Hãy tự build từ Actions.
- **Tải exe ở đâu?**
  - Từ **Actions** artifact của chính fork bạn sau khi chạy workflow **Build executable**, hoặc trang **Releases** của fork bạn nếu bạn publish (fork này có release [2.2.0.0](../../releases/tag/2.2.0.0) kèm patcher đã bật tiếng Việt). **Không** tải `.exe` từ mô tả YouTube, trang mirror lạ, Discord hay comment issue.
- **Windows Defender/SmartScreen cảnh báo build của tôi?**
  - Artifact từ Actions chưa chữ ký và hiếm nên Windows có thể cảnh báo dù code tự build từ fork của bạn. Hãy xem lại mã nguồn, đối chiếu log workflow và chỉ chạy binary bạn tự build.
- **Dùng binary người khác build được không?**
  - Được nhưng hãy coi là không đáng tin — kho này không thể xác minh hay hỗ trợ build bên thứ ba.
- **Có gửi dữ liệu đi đâu không?**
  - Patcher .NET mặc định hoàn toàn offline. Remote Web Panel (tùy chọn) chỉ nghe trên LAN của bạn và có thể xin bản dịch/ảnh minh họa trainer qua đường API/CDN sẵn có của Wand. Nếu bạn chủ động compile thông báo kiểm tra release, mỗi lần Wand khởi động chỉ gọi API release công khai của GitHub (kèm IP + User-Agent) — không có telemetri, không tải, không tự cập nhật.
- **Làm sao biết bản mới mà không bật kiểm tra cập nhật trong app?**
  - Trên GitHub chọn **Watch → Custom → Releases**, rồi sync fork và chạy **Build executable** khi có release mới. Cũng có thể bật thông báo lúc build trong workflow thủ công.

---

## 📜 License
This project is licensed under the Apache-2.0 - see the [LICENSE](LICENSE.md) file for details.


## ❤️ Support

If you find this project useful, you can support its development using any of the options below 🙌

[![Patreon](https://img.shields.io/badge/Patreon-donate-f96854.svg?logo=patreon)](https://www.patreon.com/kitbyte/gift)
[![USDT TRC20](https://img.shields.io/badge/USDT--TRC20-donate-26a17b.svg?logo=tether)](https://tronscan.org/#/address/TQdvau8pAy5Tg1Aa588tTcPCFgbcHtuoxc)
[![BTC](https://img.shields.io/badge/BTC-donate-f7931a.svg?logo=bitcoin)](https://www.blockchain.com/explorer/addresses/btc/1EZKDcyU8REm9JW5xwXJqSpn5Xaq5yAWWX)
[![ETH](https://img.shields.io/badge/ETH-donate-3c3c3d.svg?logo=ethereum)](https://etherscan.io/address/0xd904d9d0557f88bbb1c4ab3582b4ca0d8a730e8d)


---

> **Legal Disclaimer:**
> This project is a third-party enhancement tool intended solely for educational, research, and local interoperability purposes. It does not distribute any proprietary code or bypass server-side validations. All modifications are performed locally to customize the user's interface.

---
