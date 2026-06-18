import { visit } from 'unist-util-visit';

const plugin = () => {
    return (tree) => {
        visit(tree, 'link', (node) => {
            if (node.url && typeof node.url === 'string') {
                // Divide o caminho em segmentos, remove prefixos numéricos de cada diretório e remonta a URL
                node.url = node.url.split('/').map(segment => {
                    // Remove prefixo numérico apenas de diretórios (não de arquivos)
                    return segment.replace(/^\d+-/, '');
                }).join('/');
            }
        });
    };
};

export default plugin;
