#!/usr/bin/env bash
set -euo pipefail
OLD="Everywhere"
NEW="AlfredGPT"
REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$REPO_ROOT"

# Abort if uncommitted changes exist
if [ -n "$(git status --porcelain)" ]; then
  echo "Repository has uncommitted changes. Please commit or stash them before running this script." >&2
  exit 1
fi

# Find files tracked by git that contain the exact token 'Everywhere'
mapfile -t files < <(git grep -Il "\b$OLD\b" || true)
if [ ${#files[@]} -eq 0 ]; then
  echo "No files found containing '$OLD'."
  exit 0
fi

echo "Found ${#files[@]} files. Backing up and replacing..."

changed=0
for f in "${files[@]}"; do
  # Make a backup copy
  cp --preserve=mode,timestamps "$f" "$f.bak"

  # Replace exact token occurrences (word boundaries)
  sed -i "s/\b$OLD\b/$NEW/g" "$f"

  # Replace avares resource scheme occurrences (avares://Everywhere/...)
  sed -i "s|avares://$OLD/|avares://$NEW/|g" "$f"

  # Replace file/name patterns like Everywhere-Windows- or Everywhere.Windows.exe
  sed -i "s/$OLD-/$NEW-/g" "$f" || true
  sed -i "s/$OLD\./$NEW\./g" "$f" || true

  # Track if file changed compared to backup
  if ! cmp -s "$f" "$f.bak"; then
    echo "modified: $f"
    changed=$((changed+1))
  else
    rm "$f.bak"
  fi
done

if [ $changed -eq 0 ]; then
  echo "No changes were made after replacements. Cleaning up backups."
  find . -name "*.bak" -type f -delete || true
  exit 0
fi

# Stage changes and create a commit
git add -A
git commit -m "Rename app: $OLD -> $NEW (content replacement)"

echo "Done. Committed $changed changed files."

echo "Backups (*.bak) kept beside modified files. Review the commit, run build/tests, then remove .bak files when satisfied."
