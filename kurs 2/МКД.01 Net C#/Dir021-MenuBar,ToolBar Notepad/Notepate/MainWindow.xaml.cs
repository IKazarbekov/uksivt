using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Notepate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextFiles textFiles;
        public MainWindow()
        {
            InitializeComponent();
            textFiles = new TextFiles()
            {
                TabControl = tabControlFiles
            };
            textFiles.CreateEmptyFile();
            textFiles.textInfoListen += (text) =>
            {
                textBlockInfo.Text = text;
            };
        }
        public MainWindow(string path, string textFile)
        {
            InitializeComponent();
            textFiles = new TextFiles()
            {
                TabControl = tabControlFiles
            };
            textFiles.OpenFile(path, textFile);
            textFiles.textInfoListen += (text) =>
            {
                textBlockInfo.Text = text;
            };
        }

        private void Button_Click_New_File(object sender, RoutedEventArgs e)
        {
            textFiles.CreateEmptyFile();
        }
        private void Button_Click_Open_File(object sender, RoutedEventArgs e)
        {
            string path = DialogFileTxt.GetOpen();
            if (string.IsNullOrEmpty(path))
                return;
            var text = File.ReadAllText(path);
            textFiles.OpenFile(path, text);
        }
        private void Button_Click_Save_File(object sender, RoutedEventArgs e)
        {
            if (textFiles.IsPathFromCurrentItem())
            {
                string path = textFiles.GetPathFromCurrentItem();
                var text = textFiles.GetTextFromCurrentTabItem();
                File.WriteAllText(path, text);
            }
            else
            {
                Button_Click_Save_File_As(sender, e);
            }
        }
        private void Button_Click_Save_File_As(object sender, RoutedEventArgs e)
        {
            string path = DialogFileTxt.GetSave();
            if (string.IsNullOrEmpty(path))
                return;
            var text = textFiles.GetTextFromCurrentTabItem();
            File.WriteAllText(path, text);
            textFiles.CloseItem(textFiles.GetPathFromCurrentItem());
            textFiles.OpenFile(path, text);
        }
        private void Button_Click_Undo_File(object sender, RoutedEventArgs e)
        {
            textFiles.GetRichFromCurrentItem().Undo();
        }
        private void Button_Click_Redo_File(object sender, RoutedEventArgs e)
        {
            textFiles.GetRichFromCurrentItem().Redo();
        }
        private void Button_Click_Update_File(object sender, RoutedEventArgs e)
        {
            var path = textFiles.GetPathFromCurrentItem();
            textFiles.CloseItem(path);
            var text = File.ReadAllText(path);
            textFiles.OpenFile(path, text);
        }
        private void Button_Click_Help(object sender, RoutedEventArgs e)
        {
            textFiles.OpenFile("Справка", "***************\r\n*   General   *\r\n***************\r\n\r\nFeatherPad is a single-instance application by default; all its windows share the same configuration and the same process, which will terminate only if all windows are closed.\r\n\r\nHowever, if FeatherPad is started with the command-line option \"--standalone\" or \"-s\", it will have a single window with a separate process, independently of other FeatherPad windows.\r\n\r\nRarely, the standalone mode may be needed. For example, if you want to use FeatherPad as the \"git\" editor, you will need \"featherpad -s\" as the editor command because \"git\" does not work correctly with single-instance editors.\r\n\r\nUse the standalone mode with care. If a standalone window is opened alongside other windows, any change in configuration, recent files, etc., which may have been made in those windows, will not be reflected by it, and if it is the last closed window, it will overwrite those changes.\r\n\r\nThe standalone mode is also an exception to the window/tab managing rules explained in this document.\r\n\r\nEnter \"featherpad --help\" in a terminal emulator to see all command-line options.\r\n\r\nNOTE: FeatherPad makes use of D-Bus to have a single process. Therefore, in (non-Linux) systems without D-Bus, it is always in the standalone mode, which means that its windows do not know about each other.\r\n\r\n*********************\r\n*   Drag-and-Drop   *\r\n*********************\r\n\r\nFiles can be dragged and dropped into FeatherPad windows.\r\n\r\nAlso, tabs can be dragged from a window and dropped into another window or outside all FeatherPad windows. In the first case, the dropped tab will appear after the previously active tab; in the second case, a new window containing the dropped tab will be created, i.e. the tab will be detached.\r\n\r\n************\r\n*   Tabs   *\r\n************\r\n\r\nTabs can be reordered or detached by the mouse.\r\n\r\nIf there is more than one tab, each one will have a right-click menu for closing its right or left tabs. With more than one tab, it is also possible to detach the active tab by clicking the related item on the File menu or with Ctrl+T.\r\n\r\nIf a file is opened in a tab, the right-click menu of that tab will also contain two items for copying the name and path of the file as well as an item for opening its containing folder with the default file manager.\r\n\r\nFor users' comfort, double clicking on an empty area of the tab-bar creates a new tab.\r\n\r\nThere are (customizable) shortcuts for switching tabs in various ways (see the File menu), but Ctrl+Tab is not one of them because the Tab key is reserved for text editing (see Keyboard Shortcuts, near the end of this document).\r\n\r\n***********************\r\n*   File Management   *\r\n***********************\r\n\r\nUnder X11, if there is a FeatherPad window on the current virtual desktop or viewport, so that more than half of its width as well as its height is visible, files will be opened as new tabs in it; otherwise, a new window will be created. Also, when the window on the current desktop/viewport has a modal dialog, another window is created.\r\n\r\nIn this way, FeatherPad is aware of X11 virtual desktops under most Linux desktop environments, although there are exceptions (like Enlightenment).\r\n\r\nFiles are always opened after the active tab unless it is empty, in which case the first file will be opened in it. If a single file is opened, its tab will be activated, but in the case of multiple files, the active tab will not change.\r\n\r\nIf a file is opened multiple times, its second (third,…) instance will be uneditable by default and will have a light yellow or dark red background, depending on whether the default or the dark color scheme is used. To make it editable, click on the newly created 'Edit' button on the toolbar or the 'Edit' menu. After that, these two buttons will disappear again.\r\n\r\nIf the opened file is a symbolic link (symlink), the context menu of its tab will have two extra items for copying its target path and opening its target inside the current window.\r\n\r\nExecutable script files could be run from inside FeatherPad if the corresponding option is enabled in the Preferences dialog. Then also a Run button will appear on the toolbar and the File menu whenever needed. If no terminal command is used to run them, their output and error messages will be shown by a popup dialog.\r\n\r\nFeatherPad remembers recently modified or opened files, depending on which option is enabled in the Preferences dialog. It can also open the files of the last window on a session startup. However, it has a more advanced session manager, which provides the user with options for saving a session and restoring or removing saved sessions at any time and without limit.\r\n\r\n***********************************\r\n*   Sessions and Side-Pane Mode   *\r\n***********************************\r\n\r\nSessions can be saved and opened by using the Session Manager dialog. As mentioned above, there is no limit to the number of stored sessions. Each session can have any name and consist of any number of files. All files of a session are opened in the current FeatherPad window, and their cursor positions are remembered.\r\n\r\nFeatherPad also has a side-pane mode, which can be enabled either temporarily or with startup. It is most suitable for working with sessions because its file list is alphabetically ordered and can be filtered. Each pane item has a right-click menu, which contains menu-items for various jobs when there is more than one page. Items can also be removed by middle-clicking without being selected.\r\n\r\nThe side-pane mode does not have features provided by tabs — for example, tab drag-and-drop is missing from it — and conversely — for example, tabs are not sorted and cannot be filtered — but the side-pane and tab modes can be used interchangeably by means of the Side-Pane menu-item, its toolbar button or its shortcut (Ctrl+Alt+P by default), and also by middle-clicking an empty space inside the tab-bar or side-pane.\r\n\r\nTo focus the side-pane when another widget has the focus, press Ctrl+Escape. To return the focus to the editor's main view from the side-pane or anywhere else, press the Escape key.\r\n\r\n*****************\r\n*   Encodings   *\r\n*****************\r\n\r\nFeatherPad tries to guess encodings when opening files. Although it often guesses them right, there is no exact way for that. Therefore, there are some encodings in the Options menu. If you choose one, the text could be saved with it by using the item \"Save with Encoding\" on the File menu. By default, all texts are saved with UTF-8, which covers all alphabets.\r\n\r\nAs Qt6 has removed the support for the legacy encodings, they are also removed from FeatherPad. Usually, you do not need to worry about encodings; nowadays, UTF-8 is the standard and is used everywhere.\r\n\r\n*****************************\r\n*   Programming Languages   *\r\n*****************************\r\n\r\nThe programming language of a file is detected based on its mime type or name, and its syntax will be highlighted if the syntax highlighting is enabled and supports the language in question.\r\n\r\nIf a text has no programming language or its syntax is not supported, only its hyperlinks/URLs will be highlighted, and it will be possible to open them by right clicking them and activating the related menu-item or by pressing the Control key, moving the cursor over them, and clicking them while the cursor is like a pointing hand.\r\n\r\nIf the option \"Preferences → Text → Support syntax override\" is enabled and checked, a language button will be added to the status bar for overriding the original syntax or lack of it. Reloading a document restores its original syntax.\r\n\r\nThere are also options in the Preferences dialog for showing whitespaces (spaces, tabs), line and document ends, and vertical position lines when syntax highlighting is enabled (by default or temporarily).\r\n\r\nThe colors of syntax highlighting can be customized in \"Preferences → Syntax Colors\". Each syntax color may have different meanings in different programming languages, but only the most important meanings are mentioned. The color value of whitespaces can be changed in the same place.\r\n\r\n*******************************\r\n*   Searching and Replacing   *\r\n*******************************\r\n\r\nIn FeatherPad, searching and replacing are done by separate widgets for the user to be able to search one string and replace another. Moreover, a separate replacement widget may prevent an unintentional replacement.\r\n\r\nTo remove the yellow highlights after finishing a search, you could\r\n\r\n* Click on the 'Clear' icon of the search entry, or\r\n* Press Ctrl+K while the search entry has focus, or\r\n* Empty the search entry and press Enter or F3 in it, or\r\n* Hide the search bar by focusing it (with Ctrl+F) and then, pressing Ctrl+F (again).\r\n\r\nEach search entry has a search history which can be shown as a popup list by clicking its arrow or by pressing Ctrl+Up/Down when it has focus. The topmost item shows the most recent searched text. When the entry has focus, Up and Down arrow keys as well as PageUp and PageDown keys can be used for selecting history items without showing the popup list: Up and Down change the selection by one item, while PageUp and PageDown select the topmost (most recent) and bottommost (oldest) items respectively.\r\n\r\nFeatherPad can use a shared history for all search entries, whether they are in the same window or in different windows of the same FeatherPad process. By default, each search entry has a separate history but that can be changed by checking \"Preferences → Window → Use a shared search history\".\r\n\r\nThe shared search history starts with every session and is forgotten as soon as the session ends (i.e., when all windows are closed — there is no point in remembering the search history indefinitely).\r\n\r\nThe 'Replace' docked window respects the settings for 'Match Case', 'Whole Word' and 'Regular Expression' on the search bar (in the last case, the matching text should be a regular expression, while the replacing text is always an ordinary string, although capturing groups like \"\\1\", \"\\2\",… are supported in it). It can be detached from and reattached to the main window at top or bottom. To remove the green highlights after replacing text, you could either hide/close the 'Replace' docked window or do as in the case of removing yellow search highlights.\r\n\r\nThe 'Replace' docked window is never shown without the search bar because the settings of the latter are needed by the former.\r\n\r\nPressing the Escape key is the easiest way of focusing the editor's main view, without changing anything else.\r\n\r\nNOTE: The Escape key never clears the search/replacement entry because the user might want to resume searching/replacing later. To clear the search/replacement entry when it is focused, press Ctrl+K.\r\n\r\n******************************\r\n*   Selection Highlighting   *\r\n******************************\r\n\r\nIf \"Preferences → Text → Selection highlighting\" is checked, all case-sensitive and whole matches of the selected text will be highlighted by a light blue color (or dark blue when the dark color scheme is used). The selected text does not need to be a whole string but the highlighted matches are always whole strings.\r\n\r\nThe selection highlighting can be used for finding nearby whole strings quickly. It is separate from the (yellow) search highlighting and can be used besides it.\r\n\r\n***********************\r\n*   Going to A Line   *\r\n***********************\r\n\r\nThe Jump bar can be shown by clicking its item on the toolbar or the Search menu. Jumping will happen after pressing Enter while the Jump spinbox is active. If the checkbox beside it is checked, all the text between the text cursor and the target line will be selected.\r\n\r\n******************\r\n*   Status Bar   *\r\n******************\r\n\r\nThe status bar not only shows information about the opened file but can also contain other widgets when certain properties are enabled in the Preferences dialog. You could also hide it in the Preferences dialog, in which case, the item \"Document Properties\" will appear on the File menu and could show it temporarily.\r\n\r\n***********************\r\n*   Wheel Scrolling   *\r\n***********************\r\n\r\nIf the cursor is inside the text view, the speed of (mouse) wheel scrolling will be normal. If, in addition, the Shift key is pressed, the text will scroll one line per wheel turn.\r\n\r\n\"Inertial\" scrolling can be enabled in the Text section of the Preferences dialog. It creates a kind of inertia with wheel scrolling when the cursor is inside the text view.\r\n\r\nFor fast wheel scrolling, put the cursor on the vertical scrollbar. Then, each step of wheel turn moves the view by one page. If the Shift key is also pressed, the view will be moved by half the page.\r\n\r\nAlso, see the section \"Keyboard Shortcuts\" → \"Scrolling\", below.\r\n\r\n********************\r\n*   Text Tabbing   *\r\n********************\r\n\r\nA single text line could be tabbed by the Tab key and untabbed by Shift+Tab (which is also called \"BackTab\") if the cursor is at its start. If multiple lines are (partially) selected, Tab and BackTab will affect all of them, regardless of the cursor position.\r\n\r\nIf Ctrl+Tab is used, the tabulation will be done by 4 spaces instead of a tab (the number of spaces can be changed in the Preferences dialog). This is sometimes called \"soft tab\".\r\n\r\nIn FeatherPad, \"hard\" and \"soft\" tabs are not mutually exclusive because some texts may need one and some the other.\r\n\r\nWith Ctrl+Meta+Tab, the text will be tabbed by 2 spaces, while Shift+Meta+Tab is for 2-space untabbing as far as possible.\r\n\r\nAll text tabs of a document can be converted to spaces (soft tabs) by using the menu item \"Text Tabs to Spaces\" in the right-click menu or the \"Edit\" menu. This conversion is done based on the value of Preferences → Text → Text tab size, which is 4 by default. The document needs to be saved after the conversion.\r\n\r\n************************\r\n*   Column Selection   *\r\n************************\r\n\r\nA text column can be selected, starting from the current text cursor, by holding Shift+Ctrl and pressing the left mouse button anywhere inside the text, such that the positions of the current text cursor and the press point become two diagonal corners of the column.\r\n\r\nIf the left mouse button is kept pressed and Shift+Ctrl is still held, moving the mouse will change the column, until the left mouse button is released.\r\n\r\nA column is deselected when the text cursor is moved in any way (e.g. by arrow keys, or by clicking anywhere without holding Shift+Ctrl) and on some other occasions, but it is kept intact on right clicking. With a selected column, the items \"Cut\", \"Copy\", \"Paste\" and \"Delete\" in the right-click and Edit menus work on that column. Column pasting may be a little confusing at first, but it can be used for pasting a copied column on another.\r\n\r\nIf a character is typed when a column is selected, that character is inserted before it for all of its rows, without deselecting it. This property can be used, e.g., for commenting out multiple lines of codes together. Also, Backspace moves the whole column backward by removing the characters that immediately precede it, and the Delete key deletes the column.\r\n\r\nHowever, the Enter and BackTab keys do not make sense with a column, and so, they simply deselect it and do their normal jobs at the cursor position (to untab multiple lines, select them in the usual way and press the BackTab key, as explained in the section \"Text Tabbing\", above).\r\n\r\nNOTE: Column selection is useful only with unwrapped lines, although it is possible with wrapped lines too. Also, it may seem counter-intuitive with non-monospace fonts, with multi-character or double-width graphemes, or where hard tabs are used instead of soft tabs, because it is based on the number of characters, not their widths.\r\n\r\n********************\r\n*   Auto-Bracket   *\r\n********************\r\n\r\nWith \"auto-bracketing\" enabled in Preferences, if a left parenthesis, brace, square bracket or double-quote is typed, a right parenthesis, brace, square bracket or double-quote will respectively be inserted after it and the cursor will be moved between them, provided that the next character is not a letter or number. (Although double-quote is not a bracket and has identical left and right signs, it is included in this.)\r\n\r\nAlso, if any part of the text is selected from end to start, typing of a left parenthesis, brace, etc. will add a right one after the selection end, so that the selection will be put inside parentheses, braces, etc.\r\n\r\nFor user convenience, if Enter/Return is pressed after a text selection is auto-bracketed by parentheses \"(...)\" or braces \"{...}\", the bracketed text will be put below the left bracket and above the right one.\r\n\r\nThe same holds for RTL (right-to-left) texts but with right and left reversed.\r\n\r\n****************************\r\n*   Ellipsis and Em Dash   *\r\n****************************\r\n\r\nWith the corresponding option enabled in Preferences and under proper circumstances, a triple period is replaced by an ellipsis (\"…\") and a double hyphen by an em dash (long dash, \"—\") while the user is typing.\r\n\r\nThe proper circumstances depend on the pressed key and, maybe, what comes before those characters. For example, in the case of a triple period, the Space or Enter/Return key should be pressed and the triple period should not follow a period. Double hyphens are not replaced in programming languages because they may have special meanings.\r\n\r\nSome other strings may also be replaced appropriately in non-programming languages, e.g., \"->\", \">=\" and \"<=\" may be changed to \"→\", \"≥\" and \"≤\" respectively. The existence of these characters are guaranteed by all good fonts.\r\n\r\n**********************\r\n*   Spell Checking   *\r\n**********************\r\n\r\nFor spell checking, a Hunspell dictionary should be first added to Preferences → Text → Hunspell dictionary path (a Hunspell dictionary has the suffix \".dic\" and should be accompanied by an affix file with the suffix \".aff\"). Spell checking can be done by F2, but its shortcut can be customized.\r\n\r\nIf \"Ignore All\" is clicked, all instances of the word will be ignored during the current check. If you know that the word is correct, you could click \"Add To Dictionary\" and it will be saved for all checks. If \"Correct All\" is clicked, other instances of the word will be corrected in the same way when reached during the current check.\r\n\r\n**************************\r\n*   Keyboard Shortcuts   *\r\n**************************\r\n\r\nTo change a customizable shortcut, double click it and press your chosen shortcut inside the shortcut editor of the Preferences dialog. To clear a shortcut, use a modifier key (like Shift). To cancel, press the Escape key before the shortcut loses focus.\r\n\r\nAll shortcuts, except for the extra ones below, can be found on menus or as tooltips, and many of them can be customized in the Preferences dialog.\r\n\r\nNOTE: Text editing shortcuts may be different for non-Linux OS's.\r\n\r\nUseful Extra (Hidden) Shortcuts:\r\n=================================\r\n\r\nWindow:\r\n*********\r\nEscape              Focus the editor's main view without changing anything else\r\nCtrl+Escape         Focus the side-pane if existing\r\nF11                 (Un-)Fullscreen\r\n\r\nZooming:\r\n**********\r\nCtrl+=              Zoom in (also Ctrl++ or Ctrl + mouse wheel)\r\nCtrl+-              Zoom out (also Ctrl + mouse wheel)\r\nCtrl+0              Reset zooming\r\n\r\nRunning a process:\r\n********************\r\nCtrl+E              Run the executable file opened in this tab (only if enabled in Preferences)\r\nCtrl+Alt+E          Exit (kill) the above process immediately\r\n\r\nScrolling:\r\n***********\r\nShift+Mouse Wheel     If cursor is inside view, scroll up/down by one wrapped line;\r\n                      if cursor is on vertical scrollbar, scroll up/down by half the page\r\nAlt+Mouse Wheel       If cursor is inside view, scroll horizontally when horizontal\r\n                      scrollbar is visible (which may happen when wrapping is disabled)\r\nCtrl+Up/Down          Scroll up/down by one wrapped line without moving text cursor\r\nCtrl+PageUp/PageDown  Scroll up/down by one page without moving text cursor\r\n\r\nMoving text cursor:\r\n********************\r\nHome                Go to to the line start plus the indentation\r\nEnd                 Go to to the line end\r\nCtrl+Home           Go to to the text start\r\nCtrl+End            Go to to the text end\r\nRight/Left          Move the cursor one character to the right/left\r\nCtrl+Right/Left     Move the cursor one word to the right/left\r\nUp/Down             Go to the same position in the previous/next wrapped line\r\nShift+Up/Down       Go to the same position in the previous/next wrapped line while selecting text\r\nMeta+Up/Down        Go to the same position in the previous/next (real) line\r\nMeta+Shift+Up/Down  Go to the same position in the previous/next (real) line while selecting text\r\nPageUp/PageDown     Go to the same position in the previous/next page\r\n\r\nText tabulation:\r\n*****************\r\nTab                 Ordinary text tabulation (its length can be changed in Preferences)\r\nShift+Tab           BackTab (the reverse of Tab)\r\nCtrl+Tab            4-space text tabulation (can be changed in Preferences)\r\nCtrl+Meta+Tab       2-space text tabulation\r\nShift+Meta+Tab      2-space BackTab, as far as possible\r\n\r\nText editing:\r\n**************\r\nInsert              Toggle overwrite mode\r\nBackspace           Delete to the left of the text cursor\r\nCtrl+Backspace      Delete to the the start of the word\r\nDelete              Delete to the right of the text cursor (the opposite of Backspace)\r\nCtrl+Delete         Delete to the end of the word (the opposite of Ctrl+Backspace)\r\nCtrl+K              Delete to the end of the line (when the editor has focus)\r\nCtrl+Shift+Up/Down  Move the current line or selected lines upward/downward\r\nShift+Enter         Insert newline with the non-letter prefix of the current line\r\n                    (to write code comments or lists easily, for example)\r\n\r\n***********************\r\n*   Multiple Clicks   *\r\n***********************\r\n\r\nDouble click          Select a whole word\r\nCtrl + Double click   Select between spaces\r\nTriple click          Select a line without its leading and trailing whitespaces\r\nCtrl + Triple click   Select a whole line plus its trailing newline if any\r\n");
        }
        private void Button_Click_Close(object sender, RoutedEventArgs e) => this.Close();
        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            if (Printer.IsConnect)
                Printer.Print(textFiles.GetTextFromCurrentTabItem());
            else
                MessageBox.Show("Принтер не подключен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void Button_Click_Copy(object sender, RoutedEventArgs e) =>
            textFiles.GetRichFromCurrentItem().Copy();
        private void Button_Click_Paste(object sender, RoutedEventArgs e) =>
            textFiles.GetRichFromCurrentItem().Paste();
        private void Button_Click_Cut(object sender, RoutedEventArgs e) =>
            textFiles.GetRichFromCurrentItem().Cut();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите выйти? Не сохранённые изменения будут удалены", "Закрытие", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            { }
            else if (result == MessageBoxResult.No)
                e.Cancel = true;
        }
        private void MenuItem_Click_OpenFontSessings(object sender, RoutedEventArgs e)
        {
            WindowFontSessings window = new WindowFontSessings();
            window.ShowDialog();
        }
        private void Click_Left_Panel_Items(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.TabStripPlacement == Dock.Top)
                tabControlFiles.TabStripPlacement = Dock.Left;
            else
                tabControlFiles.TabStripPlacement = Dock.Top;
        }
        private void Click_Take_Out_Item(object sender, RoutedEventArgs e)
        {
            var path = textFiles.GetPathFromCurrentItem();
            var text = textFiles.GetTextFromCurrentTabItem();
            var window = new MainWindow(path, text);
            textFiles.CloseItem(path);
            window.Show();
        }
        private void Click_To_Last_Item(object sender, RoutedEventArgs e)
        {
            tabControlFiles.SelectedIndex++;
        }
        private void Click_To_Before_Item(object sender, RoutedEventArgs e)
            => textFiles.BeforeItem();
        private void Click_To_Back_Item(object sender, RoutedEventArgs e)
            => textFiles.BackItem();
        private void Click_To_First_Item(object sender, RoutedEventArgs e)
            => textFiles.FirstItem();
        private void Click_To_End_Item(object sender, RoutedEventArgs e)
            => textFiles.EndItem();
        private void Click_To_last_Item(object sender, RoutedEventArgs e)
            => textFiles.LastItem();

        private void Click_Panel_Find(object sender, RoutedEventArgs e)
            => textFiles.LastItem();

        private void buttonFindNext_Click(object sender, RoutedEventArgs e)
        {
            var pattern = textBoxFind.Text;
            textFiles.FindText(pattern, false, false, false);
        }

        private void buttonFindBack_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    // Класс - Отвечает за работу с файлами и их вкладками
    class TextFiles
    {
        private TabControl tabControl;
        public TabControl TabControl
        {
            get
            {
                return tabControl;
            }
            set
            {
                tabControl = value;
            }
        }
        private List<string> pathFiles = new List<string>();
        public delegate void TextInfoListen(string text);
        public TextInfoListen textInfoListen; // Оповещение о кол-ве букв, слов и строк
        int indexLastItem = -1;
        // Методы для перемещения вкладок
        public void BeforeItem()
        {
            int index = tabControl.SelectedIndex;
            if (index < tabControl.Items.Count)
            {
                indexLastItem = index;
                index++;
                tabControl.SelectedIndex = index;
            }
        }
        public void BackItem()
        {
            int index = tabControl.SelectedIndex;
            if (index > 0)
            {
                indexLastItem = index;
                index--;
                tabControl.SelectedIndex = index;
            }
        }
        public void FirstItem()
        {
            int index = 0;
            indexLastItem = tabControl.SelectedIndex;
            tabControl.SelectedIndex = index;
        }
        public void EndItem()
        {
            int index = tabControl.Items.Count - 1;
            indexLastItem = tabControl.SelectedIndex;
            tabControl.SelectedIndex = index;
        }
        public void LastItem() => tabControl.SelectedIndex = indexLastItem;
        // Метод для получения кол-ва вкладок
        public int GetCountItems()
        {
            return pathFiles.Count;
        }
        // Метод - Дать RichTextBox открытой вкладки
        public RichTextBox GetRichFromCurrentItem()
        {
            var item = tabControl.Items[tabControl.SelectedIndex];
            var rich = ((TabItem)item).Content as RichTextBox;
            return rich;
        }
        // Метод - Дать путь файла открытой вкладки
        public string GetPathFromCurrentItem()
        {
            return pathFiles[tabControl.SelectedIndex];
        }
        // Метод - Есть ли путь этой вкладки
        public bool IsPathFromCurrentItem()
        {
            return File.Exists(GetPathFromCurrentItem());
        }

        // Метод - Создай пустой файл
        public void CreateEmptyFile() => AddItem();

        // Метод - Открыть файл
        public void OpenFile(string path, string text) => AddItem(path, text);

        // Метод - Вернуть текст с вкладки
        public string GetTextFromItem(string path)
        {
            try
            {
                var index = pathFiles.IndexOf(path);
                var item = (TabItem)tabControl.Items.GetItemAt(index);
                var rich = (RichTextBox)item.Content;
                var text = new TextRange(rich.Document.ContentStart, rich.Document.ContentEnd).Text;
                return text;
            }
            catch
            {
                throw new FileNotFoundException("Файл не был открыт и не может быть сохранён");
            }

        }

        // Метод - Вернуть текст с открыой вкладки
        public string GetTextFromCurrentTabItem()
        {
            var index = tabControl.SelectedIndex;
            var item = (TabItem)tabControl.Items.GetItemAt(index);
            var rich = (RichTextBox)item.Content;
            var text = new TextRange(rich.Document.ContentStart, rich.Document.ContentEnd).Text;
            return text;
        }

        // Метод - Закрыть вкладку
        public void CloseItem(string path)
        {
            int index = pathFiles.IndexOf(path);
            if (index == -1)
                throw new System.Exception("Not path in tabControl");
            var item = tabControl.Items[index];
            tabControl.Items.RemoveAt(index);
            pathFiles.RemoveAt(index);
        }
        // Метод - добавить вкладку
        private void AddItem(string path = null, string nameOrText = "Без имени")
        {
            if (tabControl.Items.Count == 1)
                if (GetTextFromCurrentTabItem().Length == 0)
                {
                    CloseItem(pathFiles[0]);
                }
            RichTextBox rich = new RichTextBox() { };
            void Listen(object obj, EventArgs e)
            {
                string text = new TextRange(rich.Document.ContentStart, rich.Document.ContentEnd).Text;
                string selectionText = new TextRange(rich.Selection.Start, rich.Selection.End).Text;
                int countLine = text.Split('\n').Length - 1;
                int countWord = text.Split().Length - countLine;
                textInfoListen?.Invoke($"Строки: {countLine}  Выделенные символы: {selectionText.Length}" +
                    $"  Слова: {countWord}");
            }
            ;
            rich.TextChanged += Listen;
            rich.SelectionChanged += Listen;
            rich.Document.LineHeight = 1;
            rich.FontSize = 30;
            if (!string.IsNullOrEmpty(path))
            {
                rich.Document.Blocks.Clear();
                rich.Document.Blocks.Add(new Paragraph(new Run(nameOrText)));
                nameOrText = Path.GetFileName(path);
            }
            else
            {
                path = "not_path" + pathFiles.Count;
            }
            pathFiles.Add(path);
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };
            panel.Children.Add(new TextBlock()
            {
                Text = nameOrText
            });
            Button button = new Button()
            {
                Content = "X"
            };
            TabItem tabItem = new TabItem()
            {
                Header = panel,
                Content = rich,
                Height = 40,
                FontSize = 30,
            };
            void CloseThisItem(object sender, RoutedEventArgs e)
            {
                //CloseItem(path); Старая версия удаления
                // Теперь новая версия с замыканием
                var result = MessageBox.Show($"Вы хотите сохранить изменения в файле {path}?", "Предупреждение",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var text = new TextRange(rich.Document.ContentStart, rich.Document.ContentEnd).Text;
                        File.WriteAllText(path, text);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        return;
                    default:
                        return;
                }
                tabControl.Items.Remove(tabItem);
                pathFiles.Remove(path);
            }
            button.Click += CloseThisItem;
            panel.Children.Add(button);
            tabControl.Items.Add(tabItem);
            tabControl.SelectedIndex = tabControl.Items.Count - 1;
        }

        // Метод - подсветить следующее найденное слово по поиску
        public void FindText(string pattern, bool isIgnoreCase, bool isFullWord, bool isRegex)
        {
            var rich = GetRichFromCurrentItem();
            FlowDocument document = rich.Document;
            TextPointer startText = document.ContentStart;
            TextPointer endText = document.ContentEnd;
            string text = new TextRange(startText, endText).Text;

            //try
            //{
                Finder.FindTextByPattern(text, pattern, isIgnoreCase, isFullWord, isRegex,
                    out int start, out int end);
                start += 2;
                end += 2;

                TextPointer startFind = startText.GetPositionAtOffset(start);
                TextPointer endFind = startText.GetPositionAtOffset(end);
                rich.Selection.Select(startFind, endFind);
                rich.Focus();
            /*}
            catch(Exception ex)
            {
                return;
            }*/
        }

        // Метод - подсветить следующее найденное слово по поиску
    }

    // Класс для получения текстовых файлов от пользователя
    static class DialogFileTxt
    {
        static public string GetOpen()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Текст|*.txt|Все файлы|*.*";
            dialog.ShowDialog();
            return dialog.FileName;
        }

        static public string GetSave()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Текст|*.txt|Все файлы|*.*";
            dialog.ShowDialog();
            return dialog.FileName;
        }
    }

    // Класс для работы с принтером
    static class Printer
    {
        static public bool IsConnect
        {
            // Вы смотрите это ? А тут ничего нет :(. Но я точно знаю что принтер не подключили
            get { return false; }
        }

        static public void Print(string text)
        {
            // Я типо печатаю ;)
        }
    }

    // Класс для поиска, замены и перехода
    static class Finder
    {
        static MatchCollection matches;
        static int index = 0;
        static string lastText = null;
        static string lastPattern;
        public static void FindTextByPattern(string text, string pattern, bool isIgnoreCase, bool isFullWord, bool isRegex, out int start, out int end, bool isNext = true)
        {
            if (lastText != text && pattern != lastPattern)
            {
                matches = Regex.Matches(text, pattern);
                index = 0;
                lastText = text;
                MessageBox.Show("Awdaw");
            }

            int count = matches.Count;
            if (count > 0)
            {
                var match = matches[index];
                start = match.Index;
                end = start + match.Length;

                if (isNext)
                    if (index < count)
                        index++;
                else
                    if (index > 0)
                        index--;
                return;
            }

            throw new Exception("No find math");
        }
    }
}
