import type { SidebarsConfig } from '@docusaurus/plugin-content-docs';
import fs from 'fs';
import path from 'path';

// Sidebar for the generated API docs (DocFX -> processApiFiles -> generated/api).
// Consumed by the dedicated 'api' docs plugin instance in docusaurus.config.ts.
const processedTocFilePath = path.join(__dirname, 'generated', 'api', 'toc_processed.json');

let apiSidebarItems = [];
try {
    if (fs.existsSync(processedTocFilePath)) {
        const toc = JSON.parse(fs.readFileSync(processedTocFilePath, 'utf8'));
        apiSidebarItems = toc.apiSidebar;
    } else {
        console.warn('Warning: toc_processed.json file not found. Using empty API sidebar.');
    }
} catch (error) {
    console.error('Error loading API sidebar:', error);
}

const sidebars: SidebarsConfig = {
    apiSidebar: apiSidebarItems,
};

export default sidebars;
