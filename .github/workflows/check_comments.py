import os
import re
import subprocess

# File extensions to exclude (localization, config files, etc.)
EXCLUDED_FILES = (".json", ".yaml", ".yml", ".po", ".license", ".xml", ".resx")
# Regex to find comments (supports Python, C++, JavaScript, Rust, Go, etc.)
COMMENT_REGEX = re.compile(r"(?://|#|<!--|/\*|\*).+")

# Regex to detect non-English letters (anything that is not a-z, A-Z, 0-9, punctuation, or spaces)
NON_ENGLISH_REGEX = re.compile(r"[^\x00-\x7F]")  # Matches any non-ASCII character

def get_base_branch():
    """Gets the base branch of the current PR from GitHub Actions environment"""
    return os.getenv("GITHUB_BASE_REF", "main")  # Default to 'main' if not in CI

def get_diff():
    """Gets the diff of the current PR with the base branch"""
    base_branch = get_base_branch()
    print(f"🔍 Checking diff against: {base_branch}")

    try:
        subprocess.run(["git", "fetch", "origin", base_branch], check=True)

        result = subprocess.run(
            ["git", "diff", "--unified=0", "--name-only", f"origin/{base_branch}"],
            capture_output=True,
            text=True,
            check=False  # Allow failures but still capture output
        )

        changed_files = result.stdout.splitlines()
        return [f for f in changed_files if not f.endswith(EXCLUDED_FILES)]

    except subprocess.CalledProcessError as e:
        print(f"❌ Error running git diff: {e}")
        return []

def extract_comments_from_file(file_path):
    """Extracts comments from a file"""
    try:
        with open(file_path, "r", encoding="utf-8") as file:
            return [
                (file_path, line.strip())  # Store file name along with comment
                for line in file
                if COMMENT_REGEX.search(line)
            ]
    except Exception as e:
        print(f"⚠️ Error reading file {file_path}: {e}")
        return []

def contains_non_english(text):
    """Checks if a string contains non-English characters"""
    return bool(NON_ENGLISH_REGEX.search(text))

def detect_non_english_comments(files):
    """Filters comments that contain non-English characters"""
    non_english = []
    for file in files:
        comments = extract_comments_from_file(file)
        for file_path, comment in comments:
            if contains_non_english(comment):
                non_english.append((file_path, comment))
    return non_english

def main():
    changed_files = get_diff()
    if not changed_files:
        print("✅ No relevant files changed.")
        return

    non_english_comments = detect_non_english_comments(changed_files)
    if non_english_comments:
        print("❌ Found comments with non-English characters:")
        for file, comment in non_english_comments:
            print(f"- {file}: {comment}")
        exit(1)  # Fail the GitHub Action

    print("✅ All comments contain only English letters.")

if __name__ == "__main__":
    main()
