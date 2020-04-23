import glob
import os
import re


def main(fix):
    files = glob.glob("F:\\AR\\unmodified_3\\Assembly-CSharp\\**\\*.cs", recursive=True)
    # files = ["F:\\AR\\unmodified_3\\Assembly-CSharp\\StandardActorEffectData.cs"]

    failed = []
    for file in files:
        print(file)
        try:
            with open(file, "rt", encoding='utf-8') as f:
                code = f.read()
            code = fix(code, failed, file)
            with open(file, "wt", encoding='utf-8') as f:
                f.write(code)
        except Exception as e:
            print(e)
            failed.append(file)

    if failed:
        print("Failed files:")
        for f in failed:
            print(f)
        print("There were failed files")


action_re = re.compile(r'(public|private|protected|internal) (static )?event Action(<[^{;]+>)? ([A-Za-z0-9_]+)')
regexes = [
    # unreachable code
    (re.compile(r'\s*if\s*\(\s*1\s*==\s*0\s*\)\s*\{\s*/\*[A-Za-z :]+\*/;\s*\}'), ''),
    (re.compile(r'\s*switch \([1-9]\)\s*{\s*case 0:\s*continue;\s*\}'), ''),
    (re.compile(r'\s*(for\s*\(;;\)|while\s*\(\s*true\s*\))\s*\{\s*break;\s*\}'), ''),
    # (re.compile(r'(?:for\s*\(;;\)|while\s*\(\s*true\s*\))\s*\{\s*switch \([1-9]\)\s*{\s*(?:case 0:\s*break;\s*)*default:\s*(.*?)\s*(?:case 0:\s*break;\s*)*\}\s*\}', re.DOTALL), '\g<1>'),
    (re.compile(r'\s*if\s*\((!true|1 == 0)\)\s*{[^{}]+}'), ''),
    (re.compile(r'\s*if\s*\(true\)\s*{\s*}'), ''),

    # address comments
    # (re.compile(r'\n\t*//( \([gs]et\))?( Token: 0x[0-9A-F]+)?( RID: [0-9]+)?( RVA: 0x[0-9A-F]+)?( File Offset: 0x[0-9A-F]+)?\s*\n'), '\n'),

    # renaming entities
    # (re.compile(r'\\u([0-9A-Fa-f]+)(?![a-z_A-Z0-9]*["\'])'), r'symbol_\g<1>'),
    # (re.compile(r'<Module>'), '_Module'),
    # (re.compile(r'<>f__(mg|am)\$(cache[0-9A-F]+)'), r'f__\g<1>_\g<2>'),
    # (re.compile(r'<Remove>'), '_Remove'),
    # (re.compile(r'<IsPlayerOnDeck>'), '_IsPlayerOnDeck'),
    # (re.compile(r'(\[DebuggerBrowsable\(DebuggerBrowsableState.Never\)\]\s*public event)'), r'// \g<1>'),
    # (re.compile(r'<([^>\s]*)>c__AnonStorey'), r'\g<1>_c__AnonStorey'),
    # (re.compile(r'<([^>\s]*)>c__Iterator'), r'\g<1>_c__Iterator'),
    (re.compile(r'base\._002Ector\(\);'), r''),
    # (re.compile(r'\$this'), r'_this'),
    # (re.compile(r'bool result;\s*return result;'), r'return false;'),

    # (re.compile(r'if\s*\(([A-Za-z_:0-9.]+\.)?(_003C_003Ef__(?:mg|am)_0024cache[0-9A-F]+) == null\)\s*{\s*(?:\1)?\2\s*=\s*'), r'BLAAAH \1 \2 BLAAH'),
]

lambda_re = re.compile(r'if\s*\(([A-Za-z_:0-9.]+\.)?(_003C_003Ef__(?:mg|am)_0024cache[0-9A-F]+) == null\)\s*{\s*(?:\1)?\2\s*=\s*'
                      r'(.*?)\s*;\s*\}\s*(?=\n[^\r\n]*?(?:\1)?\2)', re.DOTALL)

def fix_events(code, failed, file):
    code = action_re.sub(r'private \2Action\3 \4Holder;\n\t\1 \2event Action\3 \4', code)
    for m in action_re.finditer(code):
        name = m.group(4)
        parent = "this" if not m.group(2) else os.path.splitext(os.path.basename(file))[0]
        code = re.sub(r"\b" + parent + r"\." + name + r"\b", parent + "." + name + "Holder", code)
    return code


def cleanup(code, failed, file):
    for regex, replacement in regexes:
        code = regex.sub(replacement, code)
    return code


def fix_lambdas(code, failed, file):
    lambdas = {}
    for m in lambda_re.finditer(code):
        name = m.group(2)
        if m.group(1) is not None:
            name = m.group(1) + name
        body = m.group(3)
        if name in lambdas and body != lambdas[name]:
            print("duplicate lambda {} in file {}".format(name, file))
            failed.append(file)
        lambdas[name] = body
    code = lambda_re.sub("", code)
    if lambdas:
        for name in lambdas:
            print("\t{}\n==================\n{}\n------------------".format(name, lambdas[name]))

        lambda_insert = "\\b(" + "|".join(re.escape(x) for x in lambdas) + ")\\b"
        print(lambda_insert)
        code = re.sub(lambda_insert, lambda match: lambdas[match.group(1)], code)
    return code


def fix_symbols(code, failed, file):
    code = re.sub(r'symbol(_[0-9A-F]{4})', r'\1', code)
    return code


if __name__ == "__main__":
    main(fix_lambdas)
    main(fix_events)
    # main(cleanup)
    # main(fix_symbols)
