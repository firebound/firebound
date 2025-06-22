const fs = require('fs');
const path = require('path');
const yaml = require('js-yaml');

const apiDir = path.join(__dirname, 'api');
const docsApiDir = path.join(__dirname, 'content/framework', 'api');
const tocFilePath = path.join(__dirname, 'api', 'toc.yml');
const processedTocFilePath = path.join(__dirname, 'content/framework', 'api', 'toc_processed.json');

// Ensure the destination directory exists
if (!fs.existsSync(docsApiDir)) {
    fs.mkdirSync(docsApiDir, { recursive: true });
}

// Function to process files
const processFiles = (dir, destDir) => {
    // Clear the destination directory
    if (fs.existsSync(destDir)) {
        fs.readdirSync(destDir).forEach(file => {
            const filePath = path.join(destDir, file);
            if (fs.lstatSync(filePath).isDirectory()) {
                fs.rmdirSync(filePath, { recursive: true });
            } else {
                fs.unlinkSync(filePath);
            }
        });
    } else {
        fs.mkdirSync(destDir, { recursive: true });
    }

    fs.readdirSync(dir).forEach(file => {
        const filePath = path.join(dir, file);
        const destFilePath = path.join(destDir, file);

        // if (file !== 'DiceRoll.md') {
        //     destFilePath = path.join(destDir, file.replace('DiceRoll.', ''));
        // }

        if (fs.lstatSync(filePath).isDirectory()) {
            // Recursively process subdirectories
            processFiles(filePath, destFilePath);
        } else if (path.extname(file) === '.md') {
            // Read the file content
            let content = fs.readFileSync(filePath, 'utf8');

            // Add frontmatter with title based on file name
            const fileName = path.basename(file, path.extname(file)).replace('DiceRolling.', '');
            const frontmatter = `---\ntitle: ${fileName}\n---\n\n`;
            content = frontmatter + content;

            // Remove "DiceRoll." from the content
            // content = content.replace(/DiceRoll\./g, '');

            // Add backslash before '<' that aren't part of HTML tags or <xref> tags
            content = content.replace(/<(?!\/?[\w\s="'-]+>|xref|a\s|\/a>)/g, '\\<');

            // Remove all <p> and </p> tags
            content = content.replace(/<\/?p>/g, '');

            // Add double spaces at the end of each line that ends with a comma
            content = content.replace(/,(?=\s*$)/gm, ',  ');

            // Remove <a>...</a> tags from headers
            // content = content.replace(/<a[^>]*>(.*?)<\/a>/g, '$1');

            // Replace <pre><code class="lang-csharp"> blocks with Markdown code blocks
            content = content.replace(/<pre><code class="lang-csharp">([\s\S]*?)<\/code><\/pre>/g, '```csharp\n$1\n```');

            // Wrap the content of `#### Inherited Members` in <details> and <summary> tags
            content = content.replace(/(#### Inherited Members\s*\n)([\s\S]*?)(?=\n#|$)/g, (match, p1, p2) => {
                return `${p1}<details>\n<summary>Show/Hide Inherited Members</summary>\n\n${p2}\n</details>`;
            });

            // Write the processed content to the destination file
            fs.writeFileSync(destFilePath, content, 'utf8');
        }
    });
};

// Function to process toc.yml
const processToc = (tocFilePath, processedTocFilePath) => {
    const toc = yaml.load(fs.readFileSync(tocFilePath, 'utf8'));

    console.log('Loaded TOC:', toc);

    const processItems = (items) => {
        if (!Array.isArray(items)) {
            return [];
        }
        return items.map(item => {
            if (item.items) {
                const category = {
                    type: 'category',
                    label: item.name,
                    items: processItems(item.items),
                };
                if (item.href) {
                    category.link = {
                        type: 'doc',
                        id: `framework/api/${item.href.replace('.md', '')}`,
                    };
                }
                return category;
            } else {
                return {
                    type: 'doc',
                    id: `framework/api/${item.href.replace('.md', '')}`,
                    label: item.name,
                };
            }
        });
    };

    const sidebar = {
        apiSidebar: processItems(toc),
    };

    console.log('Processed Sidebar:', sidebar);

    fs.writeFileSync(processedTocFilePath, JSON.stringify(sidebar, null, 2), 'utf8');
};

// Process the files in the api directory
processFiles(apiDir, docsApiDir);

// Process the toc.yml file
processToc(tocFilePath, processedTocFilePath);

console.log('Files processed and copied successfully.');