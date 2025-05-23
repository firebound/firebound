import { visit } from 'unist-util-visit';

const plugin = () => {
    return (tree) => {
        visit(tree, 'link', (node) => {
            if (node.url && typeof node.url === 'string') {
                // Regex para encontrar links relativos que começam com ../ seguido por um número e hífen
                const match = node.url.match(/^(\.\.\/)(?:\d+-)?([^\/]+)(\/.*)?$/);
                
                if (match) {
                    const prefix = match[1]; // ../
                    let directoryName = match[2]; // nome do diretório sem o número e hífen
                    const restOfPath = match[3] || ''; // o resto do caminho, incluindo a barra inicial se houver

                    // Remove o prefixo numérico se existir (ex: 00-intro -> intro)
                    directoryName = directoryName.replace(/^\d+-/, '');

                    node.url = `${prefix}${directoryName}${restOfPath}`;
                }
            }
        });
    };
};

export default plugin;
