# frozen_string_literal: true

require_relative 'regilattice/version'
require 'open-uri'
require 'fileutils'

module Regilattice
  GEM_ROOT = File.expand_path('..', __dir__)
  BIN_DIR  = File.join(GEM_ROOT, 'bin')
  BINARY   = File.join(BIN_DIR, 'regilattice.exe')

  RELEASE_URL =
    "https://github.com/RajwanYair/RegiLattice/releases/download/" \
    "v#{VERSION}/RegiLatticeCLI-v#{VERSION}-win-x64.exe"

  def self.install_binary
    FileUtils.mkdir_p(BIN_DIR)
    warn "[regilattice] Downloading CLI v#{VERSION} for Windows x64..."
    URI.parse(RELEASE_URL).open('rb') { |src| File.binwrite(BINARY, src.read) }
    warn '[regilattice] Installation complete.'
  rescue OpenURI::HTTPError => e
    warn "[regilattice] Download failed: #{e.message}"
    warn "[regilattice] Download manually from: #{RELEASE_URL}"
    exit 1
  end
end
