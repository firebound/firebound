import { visit } from 'unist-util-visit';

const plugin = () => {
    return (tree) => {
        visit(tree, 'link', (node) => {
            if (node.url && typeof node.url === 'string') {
                // Ignora links externos (http/https)
                if (node.url.startsWith('http://') || node.url.startsWith('https://')) {
                    return;
                }
                
                // Separa a URL da âncora (#section)
                const [urlPath, anchor] = node.url.split('#');
                
                // Divide o caminho em segmentos, remove prefixos numéricos de cada diretório
                let cleanedUrl = urlPath.split('/').map(segment => {
                    // Remove prefixo numérico apenas de diretórios (não de arquivos)
                    return segment.replace(/^\d+-/, '');
                }).join('/');
                
                // Remove a extensão .md (mantém .mdx)
                cleanedUrl = cleanedUrl.replace(/\.md$/, '');
                
                // Reconstrói a URL com a âncora, se existir
                node.url = anchor ? `${cleanedUrl}#${anchor}` : cleanedUrl;
            }
        });
    };
};

export default plugin;
