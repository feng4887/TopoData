#!/bin/sh
# 把 TopoView 注册进 Linux 应用菜单并安装图标。
# 用法：在解压后的程序目录（含 TopoView 可执行文件）里执行 ./install.sh
# 仅当前用户生效（~/.local），无需 root；卸载删除以下两个文件即可：
#   ~/.local/share/applications/topoview.desktop
#   ~/.local/share/icons/hicolor/64x64/apps/topoview.png
set -e
DIR="$(cd "$(dirname "$0")" && pwd)"
BIN="$DIR/TopoView"
[ -x "$BIN" ] || { echo "未找到可执行文件：$BIN（请在解压后的目录内运行）"; exit 1; }

mkdir -p "$HOME/.local/share/applications" "$HOME/.local/share/icons/hicolor/64x64/apps"
cp "$DIR/topoview.png" "$HOME/.local/share/icons/hicolor/64x64/apps/topoview.png"
sed "s|^Exec=.*|Exec=$BIN|" "$DIR/TopoView.desktop" > "$HOME/.local/share/applications/topoview.desktop"
chmod +x "$HOME/.local/share/applications/topoview.desktop"
command -v update-desktop-database >/dev/null 2>&1 && update-desktop-database "$HOME/.local/share/applications" || true

echo "安装完成："
echo "  - 应用菜单 / 活动概览搜索 “TopoView” 启动"
echo "  - 图标已装入 hicolor 主题目录"
