import { themes as prismThemes } from 'prism-react-renderer';
import type { Config } from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';
import path from 'path';
import fs from 'fs';
import remarkRemoveLinkPrefix from './src/plugins/remark-remove-link-prefix.js';

// Hand-written documentation content lives at the repo root in /docs.
// Generated API docs (DocFX -> processApiFiles) are app-internal under ./generated
// and only wired in when present (they are gitignored / built in CI).
const handwrittenDocsPath = '../../docs';
const apiEnabled = fs.existsSync(path.join(__dirname, 'generated', 'api', 'toc_processed.json'));

const config: Config = {
    title: 'Firebound Docs',
    staticDirectories: ['public'],
    favicon: 'img/favicon.svg',
    url: 'https://firebound.dev',
    baseUrl: '/',

    organizationName: 'firebound',
    projectName: 'firebound',

    onBrokenLinks: 'warn',
    onBrokenMarkdownLinks: 'warn',
    onBrokenAnchors: 'warn',

    i18n: {
        defaultLocale: 'pt',
        locales: ['pt'],
    },

    markdown: {
        mermaid: true,
    },

    themes: [
        '@docusaurus/theme-mermaid',
        '@easyops-cn/docusaurus-search-local'
    ],

    presets: [
        [
            'classic',
            {
                docs: {
                    path: handwrittenDocsPath,
                    sidebarPath: './sidebars.ts',
                    remarkPlugins: [remarkRemoveLinkPrefix],
                },
                blog: false,
                theme: {
                    customCss: './src/css/custom.css',
                },
            } satisfies Preset.Options,
        ],
    ],

    plugins: [
        // require.resolve('docusaurus-lunr-search'),
        ['./src/plugins/tailwind-config.js', {}],
        ...(apiEnabled ? [
            [
                '@docusaurus/plugin-content-docs',
                {
                    id: 'api',
                    path: './generated/api',
                    routeBasePath: 'api',
                    sidebarPath: './sidebars-api.ts',
                    remarkPlugins: [remarkRemoveLinkPrefix],
                },
            ],
        ] : []),
    ],

    themeConfig: {
        image: 'img/docusaurus-social-card.jpg',
        docs: {
            sidebar: {
                hideable: true,
            }
        },
        navbar: {
            logo: {
                alt: 'Firebound Logo',
                src: 'img/logo.svg',
            },
            items: [
                {
                    type: 'docSidebar',
                    sidebarId: 'architectureSidebar',
                    position: 'left' as const,
                    label: 'Framework',
                },
                {
                    type: 'docSidebar',
                    sidebarId: 'gameDesignSidebar',
                    position: 'left' as const,
                    label: 'Game Design',
                },
                {
                    type: 'docSidebar',
                    sidebarId: 'gameContentSidebar',
                    position: 'left' as const,
                    label: 'Game Content',
                },
                {
                    type: 'docSidebar',
                    sidebarId: 'tutorialsSidebar',
                    position: 'left' as const,
                    label: 'Tutoriais',
                },
                ...(apiEnabled ? [
                    {
                        type: 'docSidebar' as const,
                        docsPluginId: 'api',
                        sidebarId: 'apiSidebar',
                        position: 'left' as const,
                        label: 'API',
                    }
                ] : []),
                {
                    href: 'https://github.com/firebound/firebound',
                    label: 'GitHub',
                    position: 'right' as const,
                },
            ],
        },
        footer: {
            style: 'dark',
            copyright: `Copyright © ${new Date().getFullYear()} Space Wizard Studios`,
        },
        prism: {
            theme: prismThemes.github,
            darkTheme: prismThemes.dracula,
            additionalLanguages: ['powershell', 'csharp'],
        },
        tableOfContents: {
            minHeadingLevel: 2,
            maxHeadingLevel: 4,
        },
    } satisfies Preset.ThemeConfig,

};

export default config;
